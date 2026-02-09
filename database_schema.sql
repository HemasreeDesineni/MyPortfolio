-- ============================================================================
-- PHOTOGRAPHY PORTFOLIO DATABASE SCHEMA - PostgreSQL
-- Database-First Approach with Dapper + CQRS
-- ============================================================================

-- Create database (run this separately if needed)
-- CREATE DATABASE photography_portfolio;

-- ============================================================================
-- 1. USERS TABLE
-- ============================================================================
CREATE TABLE users (
    id VARCHAR(36) PRIMARY KEY DEFAULT gen_random_uuid()::TEXT,
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    email VARCHAR(256) NOT NULL UNIQUE,
    phone_number VARCHAR(20) NULL,
    password_hash TEXT NOT NULL,
    email_confirmed BOOLEAN NOT NULL DEFAULT false,
    phone_confirmed BOOLEAN NOT NULL DEFAULT false,
    two_factor_enabled BOOLEAN NOT NULL DEFAULT false,
    lockout_end TIMESTAMP NULL,
    lockout_enabled BOOLEAN NOT NULL DEFAULT true,
    access_failed_count INTEGER NOT NULL DEFAULT 0,
    security_stamp VARCHAR(256) NULL,
    concurrency_stamp VARCHAR(256) NULL,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NULL,
    last_login_at TIMESTAMP NULL,
    is_active BOOLEAN NOT NULL DEFAULT true
);

-- Indexes for users table
CREATE INDEX idx_users_email ON users(email);
CREATE INDEX idx_users_phone ON users(phone_number);
CREATE INDEX idx_users_created_at ON users(created_at);
CREATE INDEX idx_users_last_login ON users(last_login_at);

-- ============================================================================
-- 2. USER ROLES TABLE
-- ============================================================================
CREATE TABLE user_roles (
    user_id VARCHAR(36) NOT NULL,
    role_name VARCHAR(50) NOT NULL,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    created_by VARCHAR(36) NULL,
    PRIMARY KEY (user_id),
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE,
    FOREIGN KEY (created_by) REFERENCES users(id) ON DELETE SET NULL
);

-- Index for user_roles table
CREATE INDEX idx_user_roles_role_name ON user_roles(role_name);

-- ============================================================================
-- 3. CATEGORIES TABLE
-- ============================================================================
CREATE TABLE categories (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE,
    slug VARCHAR(120) NOT NULL UNIQUE,
    description VARCHAR(500) NULL,
    meta_title VARCHAR(160) NULL,
    meta_description VARCHAR(320) NULL,
    cover_image_url VARCHAR(500) NULL,
    is_active BOOLEAN NOT NULL DEFAULT true,
    is_featured BOOLEAN NOT NULL DEFAULT false,
    sort_order INTEGER NOT NULL DEFAULT 0,
    parent_id INTEGER NULL,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NULL,
    created_by VARCHAR(36) NULL,
    updated_by VARCHAR(36) NULL,
    FOREIGN KEY (parent_id) REFERENCES categories(id) ON DELETE SET NULL,
    FOREIGN KEY (created_by) REFERENCES users(id) ON DELETE SET NULL,
    FOREIGN KEY (updated_by) REFERENCES users(id) ON DELETE SET NULL
);

-- Indexes for categories table
CREATE INDEX idx_categories_slug ON categories(slug);
CREATE INDEX idx_categories_parent_id ON categories(parent_id);
CREATE INDEX idx_categories_is_active ON categories(is_active);
CREATE INDEX idx_categories_is_featured ON categories(is_featured);
CREATE INDEX idx_categories_sort_order ON categories(sort_order);

-- ============================================================================
-- 4. PHOTOS TABLE
-- ============================================================================
CREATE TABLE photos (
    id SERIAL PRIMARY KEY,
    title VARCHAR(200) NOT NULL,
    slug VARCHAR(250) NOT NULL,
    description TEXT NULL,
    alt_text VARCHAR(255) NULL,
    file_name VARCHAR(500) NOT NULL,
    file_path VARCHAR(500) NOT NULL,
    thumbnail_path VARCHAR(500) NULL,
    medium_path VARCHAR(500) NULL,
    large_path VARCHAR(500) NULL,
    original_file_name VARCHAR(500) NOT NULL,
    file_size BIGINT NOT NULL DEFAULT 0,
    file_size_formatted VARCHAR(20) NULL,
    content_type VARCHAR(50) NOT NULL,
    image_width INTEGER NULL,
    image_height INTEGER NULL,
    aspect_ratio DECIMAL(10,4) NULL,
    camera_make VARCHAR(100) NULL,
    camera_model VARCHAR(100) NULL,
    lens VARCHAR(100) NULL,
    focal_length VARCHAR(20) NULL,
    aperture VARCHAR(10) NULL,
    shutter_speed VARCHAR(20) NULL,
    iso VARCHAR(10) NULL,
    taken_at TIMESTAMP NULL,
    location VARCHAR(200) NULL,
    latitude DECIMAL(10,8) NULL,
    longitude DECIMAL(11,8) NULL,
    category_id INTEGER NOT NULL,
    is_active BOOLEAN NOT NULL DEFAULT true,
    is_featured BOOLEAN NOT NULL DEFAULT false,
    is_private BOOLEAN NOT NULL DEFAULT false,
    allow_download BOOLEAN NOT NULL DEFAULT false,
    sort_order INTEGER NOT NULL DEFAULT 0,
    view_count INTEGER NOT NULL DEFAULT 0,
    like_count INTEGER NOT NULL DEFAULT 0,
    download_count INTEGER NOT NULL DEFAULT 0,
    tags TEXT[] NULL,
    colors VARCHAR(50)[] NULL,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NULL,
    published_at TIMESTAMP NULL,
    created_by VARCHAR(36) NULL,
    updated_by VARCHAR(36) NULL,
    FOREIGN KEY (category_id) REFERENCES categories(id) ON DELETE RESTRICT,
    FOREIGN KEY (created_by) REFERENCES users(id) ON DELETE SET NULL,
    FOREIGN KEY (updated_by) REFERENCES users(id) ON DELETE SET NULL
);

-- Indexes for photos table
CREATE UNIQUE INDEX idx_photos_slug ON photos(slug);
CREATE INDEX idx_photos_category_id ON photos(category_id);
CREATE INDEX idx_photos_is_active ON photos(is_active);
CREATE INDEX idx_photos_is_featured ON photos(is_featured);
CREATE INDEX idx_photos_is_private ON photos(is_private);
CREATE INDEX idx_photos_sort_order ON photos(sort_order);
CREATE INDEX idx_photos_created_at ON photos(created_at);
CREATE INDEX idx_photos_published_at ON photos(published_at);
CREATE INDEX idx_photos_view_count ON photos(view_count);
CREATE INDEX idx_photos_like_count ON photos(like_count);
CREATE INDEX idx_photos_tags ON photos USING GIN(tags);
CREATE INDEX idx_photos_colors ON photos USING GIN(colors);
CREATE INDEX idx_photos_taken_at ON photos(taken_at);

-- ============================================================================
-- 5. APPOINTMENTS TABLE
-- ============================================================================
CREATE TABLE appointments (
    id SERIAL PRIMARY KEY,
    appointment_number VARCHAR(20) NOT NULL UNIQUE,
    user_id VARCHAR(36) NOT NULL,
    event_type VARCHAR(200) NOT NULL,
    event_category VARCHAR(100) NULL,
    requested_date DATE NOT NULL,
    requested_time TIME NULL,
    requested_datetime TIMESTAMP NULL,
    duration_hours DECIMAL(4,2) NULL DEFAULT 2.00,
    location VARCHAR(200) NULL,
    venue_name VARCHAR(150) NULL,
    venue_address TEXT NULL,
    venue_contact VARCHAR(100) NULL,
    description TEXT NULL,
    special_requirements TEXT NULL,
    guest_count INTEGER NULL,
    client_name VARCHAR(100) NOT NULL,
    client_email VARCHAR(200) NOT NULL,
    client_phone VARCHAR(20) NULL,
    client_address TEXT NULL,
    emergency_contact VARCHAR(200) NULL,
    emergency_phone VARCHAR(20) NULL,
    status INTEGER NOT NULL DEFAULT 0, -- 0=Pending, 1=Approved, 2=Rejected, 3=Completed, 4=Cancelled
    priority INTEGER NOT NULL DEFAULT 1, -- 1=Low, 2=Medium, 3=High, 4=Urgent
    admin_notes TEXT NULL,
    rejection_reason TEXT NULL,
    cancellation_reason TEXT NULL,
    estimated_cost DECIMAL(10,2) NULL,
    final_cost DECIMAL(10,2) NULL,
    deposit_amount DECIMAL(10,2) NULL,
    payment_required BOOLEAN NOT NULL DEFAULT false,
    deposit_required BOOLEAN NOT NULL DEFAULT false,
    contract_signed BOOLEAN NOT NULL DEFAULT false,
    contract_url VARCHAR(500) NULL,
    payment_id INTEGER NULL,
    assigned_photographer VARCHAR(36) NULL,
    equipment_list TEXT NULL,
    travel_required BOOLEAN NOT NULL DEFAULT false,
    travel_distance_km DECIMAL(6,2) NULL,
    travel_cost DECIMAL(8,2) NULL,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NULL,
    approved_at TIMESTAMP NULL,
    rejected_at TIMESTAMP NULL,
    completed_at TIMESTAMP NULL,
    cancelled_at TIMESTAMP NULL,
    reminder_sent_at TIMESTAMP NULL,
    follow_up_sent_at TIMESTAMP NULL,
    created_by VARCHAR(36) NULL,
    updated_by VARCHAR(36) NULL,
    approved_by VARCHAR(36) NULL,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE CASCADE,
    FOREIGN KEY (assigned_photographer) REFERENCES users(id) ON DELETE SET NULL,
    FOREIGN KEY (created_by) REFERENCES users(id) ON DELETE SET NULL,
    FOREIGN KEY (updated_by) REFERENCES users(id) ON DELETE SET NULL,
    FOREIGN KEY (approved_by) REFERENCES users(id) ON DELETE SET NULL
);

-- Indexes for appointments table
CREATE UNIQUE INDEX idx_appointments_number ON appointments(appointment_number);
CREATE INDEX idx_appointments_user_id ON appointments(user_id);
CREATE INDEX idx_appointments_status ON appointments(status);
CREATE INDEX idx_appointments_priority ON appointments(priority);
CREATE INDEX idx_appointments_requested_date ON appointments(requested_date);
CREATE INDEX idx_appointments_created_at ON appointments(created_at);
CREATE INDEX idx_appointments_approved_at ON appointments(approved_at);
CREATE INDEX idx_appointments_event_type ON appointments(event_type);
CREATE INDEX idx_appointments_client_email ON appointments(client_email);
CREATE INDEX idx_appointments_assigned_photographer ON appointments(assigned_photographer);

-- ============================================================================
-- 6. PAYMENTS TABLE
-- ============================================================================
CREATE TABLE payments (
    id SERIAL PRIMARY KEY,
    payment_number VARCHAR(20) NOT NULL UNIQUE,
    appointment_id INTEGER NOT NULL UNIQUE,
    payment_type VARCHAR(50) NOT NULL DEFAULT 'full', -- 'deposit', 'partial', 'full', 'refund'
    amount DECIMAL(10,2) NOT NULL,
    currency VARCHAR(10) NOT NULL DEFAULT 'USD',
    tax_amount DECIMAL(10,2) NULL DEFAULT 0.00,
    fee_amount DECIMAL(10,2) NULL DEFAULT 0.00,
    net_amount DECIMAL(10,2) NULL,
    status INTEGER NOT NULL DEFAULT 0, -- 0=Pending, 1=Processing, 2=Succeeded, 3=Failed, 4=Cancelled, 5=Refunded
    payment_method VARCHAR(200) NULL,
    payment_provider VARCHAR(100) NULL DEFAULT 'stripe',
    transaction_id VARCHAR(200) NULL,
    payment_intent_id VARCHAR(200) NULL,
    charge_id VARCHAR(200) NULL,
    refund_id VARCHAR(200) NULL,
    customer_id VARCHAR(200) NULL,
    invoice_number VARCHAR(50) NULL,
    invoice_url VARCHAR(500) NULL,
    receipt_url VARCHAR(500) NULL,
    description VARCHAR(500) NULL,
    payment_metadata JSONB NULL,
    billing_address JSONB NULL,
    card_last_four VARCHAR(4) NULL,
    card_brand VARCHAR(20) NULL,
    card_exp_month INTEGER NULL,
    card_exp_year INTEGER NULL,
    failure_code VARCHAR(100) NULL,
    failure_message TEXT NULL,
    dispute_status VARCHAR(50) NULL,
    refund_reason VARCHAR(200) NULL,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    processed_at TIMESTAMP NULL,
    paid_at TIMESTAMP NULL,
    failed_at TIMESTAMP NULL,
    cancelled_at TIMESTAMP NULL,
    refunded_at TIMESTAMP NULL,
    updated_at TIMESTAMP NULL,
    created_by VARCHAR(36) NULL,
    updated_by VARCHAR(36) NULL,
    FOREIGN KEY (appointment_id) REFERENCES appointments(id) ON DELETE CASCADE,
    FOREIGN KEY (created_by) REFERENCES users(id) ON DELETE SET NULL,
    FOREIGN KEY (updated_by) REFERENCES users(id) ON DELETE SET NULL
);

-- Indexes for payments table
CREATE UNIQUE INDEX idx_payments_number ON payments(payment_number);
CREATE UNIQUE INDEX idx_payments_appointment_id ON payments(appointment_id);
CREATE INDEX idx_payments_status ON payments(status);
CREATE INDEX idx_payments_payment_type ON payments(payment_type);
CREATE INDEX idx_payments_transaction_id ON payments(transaction_id);
CREATE INDEX idx_payments_payment_intent_id ON payments(payment_intent_id);
CREATE INDEX idx_payments_created_at ON payments(created_at);
CREATE INDEX idx_payments_paid_at ON payments(paid_at);
CREATE INDEX idx_payments_invoice_number ON payments(invoice_number);

-- ============================================================================
-- 7. PHOTO GALLERIES TABLE (Optional - for organizing photos into galleries)
-- ============================================================================
CREATE TABLE photo_galleries (
    id SERIAL PRIMARY KEY,
    name VARCHAR(150) NOT NULL,
    slug VARCHAR(170) NOT NULL UNIQUE,
    description TEXT NULL,
    cover_photo_id INTEGER NULL,
    is_active BOOLEAN NOT NULL DEFAULT true,
    is_public BOOLEAN NOT NULL DEFAULT true,
    is_featured BOOLEAN NOT NULL DEFAULT false,
    password_protected BOOLEAN NOT NULL DEFAULT false,
    access_password VARCHAR(255) NULL,
    sort_order INTEGER NOT NULL DEFAULT 0,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NULL,
    published_at TIMESTAMP NULL,
    created_by VARCHAR(36) NULL,
    updated_by VARCHAR(36) NULL,
    FOREIGN KEY (cover_photo_id) REFERENCES photos(id) ON DELETE SET NULL,
    FOREIGN KEY (created_by) REFERENCES users(id) ON DELETE SET NULL,
    FOREIGN KEY (updated_by) REFERENCES users(id) ON DELETE SET NULL
);

-- ============================================================================
-- 8. PHOTO_GALLERY_ITEMS TABLE (Many-to-Many relationship)
-- ============================================================================
CREATE TABLE photo_gallery_items (
    id SERIAL PRIMARY KEY,
    gallery_id INTEGER NOT NULL,
    photo_id INTEGER NOT NULL,
    sort_order INTEGER NOT NULL DEFAULT 0,
    added_at TIMESTAMP NOT NULL DEFAULT NOW(),
    added_by VARCHAR(36) NULL,
    UNIQUE(gallery_id, photo_id),
    FOREIGN KEY (gallery_id) REFERENCES photo_galleries(id) ON DELETE CASCADE,
    FOREIGN KEY (photo_id) REFERENCES photos(id) ON DELETE CASCADE,
    FOREIGN KEY (added_by) REFERENCES users(id) ON DELETE SET NULL
);

-- Indexes for photo_gallery_items table
CREATE INDEX idx_photo_gallery_items_gallery_id ON photo_gallery_items(gallery_id);
CREATE INDEX idx_photo_gallery_items_photo_id ON photo_gallery_items(photo_id);
CREATE INDEX idx_photo_gallery_items_sort_order ON photo_gallery_items(sort_order);

-- ============================================================================
-- 9. CLIENT_REVIEWS TABLE (Optional - for client testimonials)
-- ============================================================================
CREATE TABLE client_reviews (
    id SERIAL PRIMARY KEY,
    appointment_id INTEGER NULL,
    user_id VARCHAR(36) NULL,
    client_name VARCHAR(100) NOT NULL,
    client_email VARCHAR(200) NULL,
    rating INTEGER NOT NULL CHECK (rating >= 1 AND rating <= 5),
    review_title VARCHAR(200) NULL,
    review_text TEXT NOT NULL,
    is_approved BOOLEAN NOT NULL DEFAULT false,
    is_featured BOOLEAN NOT NULL DEFAULT false,
    is_public BOOLEAN NOT NULL DEFAULT false,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    approved_at TIMESTAMP NULL,
    approved_by VARCHAR(36) NULL,
    FOREIGN KEY (appointment_id) REFERENCES appointments(id) ON DELETE SET NULL,
    FOREIGN KEY (user_id) REFERENCES users(id) ON DELETE SET NULL,
    FOREIGN KEY (approved_by) REFERENCES users(id) ON DELETE SET NULL
);

-- Indexes for client_reviews table
CREATE INDEX idx_client_reviews_appointment_id ON client_reviews(appointment_id);
CREATE INDEX idx_client_reviews_user_id ON client_reviews(user_id);
CREATE INDEX idx_client_reviews_rating ON client_reviews(rating);
CREATE INDEX idx_client_reviews_is_approved ON client_reviews(is_approved);
CREATE INDEX idx_client_reviews_is_featured ON client_reviews(is_featured);

-- ============================================================================
-- 10. CONTACT_INQUIRIES TABLE (For general contact form submissions)
-- ============================================================================
CREATE TABLE contact_inquiries (
    id SERIAL PRIMARY KEY,
    inquiry_number VARCHAR(20) NOT NULL UNIQUE,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(200) NOT NULL,
    phone VARCHAR(20) NULL,
    company VARCHAR(150) NULL,
    subject VARCHAR(200) NOT NULL,
    message TEXT NOT NULL,
    inquiry_type VARCHAR(50) NULL DEFAULT 'general', -- 'general', 'booking', 'pricing', 'collaboration'
    preferred_contact_method VARCHAR(20) NULL DEFAULT 'email', -- 'email', 'phone', 'both'
    budget_range VARCHAR(50) NULL,
    event_date DATE NULL,
    source VARCHAR(100) NULL, -- 'website', 'social_media', 'referral', 'google'
    ip_address INET NULL,
    user_agent TEXT NULL,
    status VARCHAR(20) NOT NULL DEFAULT 'new', -- 'new', 'in_progress', 'replied', 'closed'
    is_spam BOOLEAN NOT NULL DEFAULT false,
    replied BOOLEAN NOT NULL DEFAULT false,
    replied_at TIMESTAMP NULL,
    replied_by VARCHAR(36) NULL,
    created_at TIMESTAMP NOT NULL DEFAULT NOW(),
    updated_at TIMESTAMP NULL,
    FOREIGN KEY (replied_by) REFERENCES users(id) ON DELETE SET NULL
);

-- Indexes for contact_inquiries table
CREATE UNIQUE INDEX idx_contact_inquiries_number ON contact_inquiries(inquiry_number);
CREATE INDEX idx_contact_inquiries_email ON contact_inquiries(email);
CREATE INDEX idx_contact_inquiries_status ON contact_inquiries(status);
CREATE INDEX idx_contact_inquiries_inquiry_type ON contact_inquiries(inquiry_type);
CREATE INDEX idx_contact_inquiries_created_at ON contact_inquiries(created_at);
CREATE INDEX idx_contact_inquiries_event_date ON contact_inquiries(event_date);

-- ============================================================================
-- INITIAL DATA SEEDING
-- ============================================================================

-- Insert default categories
INSERT INTO categories (name, slug, description, is_active, sort_order, created_at) VALUES
('Portraits', 'portraits', 'Professional portrait photography sessions', true, 1, NOW()),
('Weddings', 'weddings', 'Wedding photography and events coverage', true, 2, NOW()),
('Events', 'events', 'Corporate and social events photography', true, 3, NOW()),
('Fashion', 'fashion', 'Fashion and commercial photography shoots', true, 4, NOW()),
('Nature', 'nature', 'Landscape and nature photography', true, 5, NOW()),
('Architecture', 'architecture', 'Architectural and interior photography', true, 6, NOW())
ON CONFLICT (name) DO NOTHING;

-- Insert default admin user (password: Admin123!)
INSERT INTO users (id, first_name, last_name, email, password_hash, email_confirmed, is_active, created_at) VALUES
('admin-user-id-001', 'Admin', 'User', 'admin@photography.com', '$2a$11$rQZt3QWXxW0IKkzq8Vg7g.5cJXKJxW0Pz9pKjHgFdSaWe9qRtYuVS', true, true, NOW())
ON CONFLICT (email) DO NOTHING;

-- Insert admin role
INSERT INTO user_roles (user_id, role_name, created_at) VALUES
('admin-user-id-001', 'Admin', NOW())
ON CONFLICT (user_id) DO NOTHING;

-- ============================================================================
-- UTILITY FUNCTIONS AND TRIGGERS
-- ============================================================================

-- Function to update updated_at timestamp
CREATE OR REPLACE FUNCTION update_updated_at_column()
RETURNS TRIGGER AS $$
BEGIN
    NEW.updated_at = NOW();
    RETURN NEW;
END;
$$ language 'plpgsql';

-- Create triggers for updated_at columns
CREATE TRIGGER update_users_updated_at BEFORE UPDATE ON users FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();
CREATE TRIGGER update_categories_updated_at BEFORE UPDATE ON categories FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();
CREATE TRIGGER update_photos_updated_at BEFORE UPDATE ON photos FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();
CREATE TRIGGER update_appointments_updated_at BEFORE UPDATE ON appointments FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();
CREATE TRIGGER update_payments_updated_at BEFORE UPDATE ON payments FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();
CREATE TRIGGER update_photo_galleries_updated_at BEFORE UPDATE ON photo_galleries FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();
CREATE TRIGGER update_contact_inquiries_updated_at BEFORE UPDATE ON contact_inquiries FOR EACH ROW EXECUTE FUNCTION update_updated_at_column();

-- Function to generate unique numbers
CREATE OR REPLACE FUNCTION generate_appointment_number()
RETURNS TEXT AS $$
BEGIN
    RETURN 'APT-' || TO_CHAR(NOW(), 'YYYY') || '-' || LPAD(NEXTVAL('appointments_id_seq')::TEXT, 6, '0');
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION generate_payment_number()
RETURNS TEXT AS $$
BEGIN
    RETURN 'PAY-' || TO_CHAR(NOW(), 'YYYY') || '-' || LPAD(NEXTVAL('payments_id_seq')::TEXT, 6, '0');
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION generate_inquiry_number()
RETURNS TEXT AS $$
BEGIN
    RETURN 'INQ-' || TO_CHAR(NOW(), 'YYYY') || '-' || LPAD(NEXTVAL('contact_inquiries_id_seq')::TEXT, 6, '0');
END;
$$ LANGUAGE plpgsql;

-- ============================================================================
-- VIEWS FOR COMMON QUERIES
-- ============================================================================

-- View for user details with roles
CREATE OR REPLACE VIEW v_users_with_roles AS
SELECT 
    u.id,
    u.first_name,
    u.last_name,
    u.email,
    u.phone_number,
    u.email_confirmed,
    u.phone_confirmed,
    u.is_active,
    u.created_at,
    u.last_login_at,
    ur.role_name
FROM users u
LEFT JOIN user_roles ur ON u.id = ur.user_id;

-- View for photos with category details
CREATE OR REPLACE VIEW v_photos_with_categories AS
SELECT 
    p.*,
    c.name AS category_name,
    c.slug AS category_slug,
    c.description AS category_description
FROM photos p
INNER JOIN categories c ON p.category_id = c.id;

-- View for appointments with user details
CREATE OR REPLACE VIEW v_appointments_detailed AS
SELECT 
    a.*,
    u.first_name || ' ' || u.last_name AS user_full_name,
    u.email AS user_email,
    p.payment_number,
    p.status AS payment_status,
    p.amount AS payment_amount
FROM appointments a
INNER JOIN users u ON a.user_id = u.id
LEFT JOIN payments p ON a.id = p.appointment_id;

-- ============================================================================
-- END OF SCHEMA
-- ============================================================================
