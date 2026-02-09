# Database Initialization Issue - Explanation

## The Problem

You're getting this error:
```
Npgsql.PostgresException: 23505: duplicate key value violates unique constraint "categories_slug_key"
```

## Root Cause

There's a **mismatch** between your table definition and your INSERT statements in `InitializeDatabaseAsync()`:

### 1. Categories Table Definition (Missing `slug` column)
```sql
CREATE TABLE IF NOT EXISTS categories (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE,
    description VARCHAR(500),
    is_active BOOLEAN NOT NULL DEFAULT true,
    sort_order INTEGER NOT NULL DEFAULT 0,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP
);
```

### 2. INSERT Statement (References `slug` column that doesn't exist in CREATE TABLE)
```sql
INSERT INTO categories (name, slug, description, is_active, sort_order, created_at) VALUES
('Portraits', 'portraits', 'Professional portrait photography', true, 1, NOW()),
...
```

### 3. Photos Table - Same Issue
The photos INSERT statement also references columns like `slug`, `alt_text`, `medium_path`, `large_path`, `original_file_name`, `file_size_formatted`, `image_width`, `image_height`, `aspect_ratio`, `camera_make`, `camera_model`, `lens`, `focal_length`, `aperture`, `shutter_speed`, `iso`, `taken_at`, `location`, `latitude`, `longitude`, `is_featured`, `is_private`, `allow_download`, `view_count`, `like_count`, `download_count`, `tags`, `colors`, `published_at`, `created_by` - **NONE of which are defined in the CREATE TABLE statement!**

## Why This Code Exists

The `InitializeDatabaseAsync()` method runs **every time the application starts** because:

1. **Development Convenience**: Automatically sets up the database on first run
2. **Idempotent Operations**: Uses `CREATE TABLE IF NOT EXISTS` to avoid errors if tables already exist
3. **Quick Prototyping**: Easier than setting up migration tools initially

However, this approach has **serious problems**:
- ❌ Column definitions don't match actual usage
- ❌ Runs on every startup (performance hit)
- ❌ Can cause conflicts with existing data
- ❌ Not suitable for production

## Solutions for Dapper + DB-First Approach

Since you're using **Dapper with a Database-First approach**, here are the appropriate solutions:

### Option 1: Remove the Initialization Code (Recommended)
Since your database schema already exists (DB-first approach), this code is unnecessary:

**In Program.cs, comment out or remove:**
```csharp
// Initialize database (create tables if they don't exist)
// await InitializeDatabaseAsync(dbConnectionFactory);  // ← COMMENT THIS OUT
```

**And remove the entire method:**
```csharp
// static async Task InitializeDatabaseAsync(IDbConnectionFactory dbConnectionFactory)
// {
//     ... entire method can be removed
// }
```

### Option 2: One-Time Data Seeding (If Needed)
If you need to seed data, create a separate one-time script or check if data exists:

```csharp
// Only seed admin user if needed
var adminEmail = "admin@photography.com";
var adminUser = await userRepository.GetByEmailAsync(adminEmail);
if (adminUser == null)
{
    // Create admin user...
}

// Only seed categories if they don't exist
using (var connection = await dbConnectionFactory.CreateConnectionAsync())
{
    var categoryCount = await connection.ExecuteScalarAsync<int>(
        "SELECT COUNT(*) FROM categories");
    
    if (categoryCount == 0)
    {
        // Seed categories with a separate script file
        await SeedInitialDataAsync(connection);
    }
}
```

### Option 3: Maintain Schema Changes with SQL Scripts
For DB-first with Dapper:
1. **Keep your schema in `database_schema.sql`** (which you already have)
2. **Run schema changes manually** via PostgreSQL client or scripts
3. **Version control your SQL scripts** with numbered files:
   - `001_initial_schema.sql`
   - `002_add_slug_column.sql`
   - `003_add_photo_metadata_columns.sql`
4. **Track applied migrations** in a simple `schema_migrations` table

## Recommended Action for DB-First Approach

1. **Immediately**: Comment out `await InitializeDatabaseAsync(dbConnectionFactory);` in Program.cs
2. **Schema Management**: Use your existing `database_schema.sql` file for schema changes
3. **Data Seeding**: Keep only the admin user seeding (with existence check), remove table creation code
4. **Future Changes**: Apply schema changes directly to database, then update your models/DTOs to match

Your `database_schema.sql` file should be the **single source of truth** for your schema, not the code in Program.cs.
