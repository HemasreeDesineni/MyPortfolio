# Portfolio Website Project Plan
## Similar to Rishi Raj Studio using Vue.js + ASP.NET Core + Dapper + PostgreSQL

## Website Analysis - Rishi Raj Studio
**Key Features Identified:**
- Full-screen hero section with background image
- Clean typography with centered branding
- "Portfolio" call-to-action button
- Dark theme sections
- Professional photography galleries
- Minimalist design approach
- Responsive layout
- Smooth scrolling/transitions

## Current Backend Status ✅
**Already Implemented:**
- ASP.NET Core 8 API
- Dapper ORM with PostgreSQL
- Repository pattern
- JWT Authentication
- Controllers: Auth, Photos, Categories
- Models: User, Photo, Category, Appointment, Payment
- AutoMapper configuration
- CORS setup for frontend integration

## Project Architecture

### Frontend (Vue.js 3)
```
portfolio-frontend/
├── src/
│   ├── components/
│   │   ├── common/
│   │   ├── layout/
│   │   └── portfolio/
│   ├── views/
│   ├── router/
│   ├── store/
│   ├── services/
│   ├── assets/
│   └── styles/
```

### Technology Stack
- **Frontend:** Vue.js 3, Vue Router, Pinia, Axios, Tailwind CSS
- **Backend:** ASP.NET Core 8 (✅ Existing)
- **Database:** PostgreSQL with Dapper (✅ Existing)
- **Authentication:** JWT (✅ Existing)
- **Deployment:** Nginx, Docker (Optional)

## Database Enhancements Needed

### Additional Tables
1. **Portfolio Sections** - Group photos by portfolio type
2. **Site Settings** - Dynamic content management
3. **Contact Inquiries** - Contact form submissions
4. **SEO Metadata** - Page-specific SEO data

## Implementation Phases

### Phase 1: Frontend Setup & Core Structure
- Initialize Vue.js 3 project
- Setup Tailwind CSS
- Configure Vue Router
- Setup Pinia store
- Create basic layout components

### Phase 2: Hero Section & Landing Page
- Full-screen hero component
- Background image integration
- Typography matching design
- Portfolio button with smooth scroll
- Navigation menu

### Phase 3: Portfolio Gallery System
- Photo gallery components
- Category filtering
- Image optimization
- Lightbox/modal functionality
- Infinite scroll/pagination

### Phase 4: Backend Integration
- API service layer
- Authentication integration
- Photo upload functionality
- Category management
- User management

### Phase 5: Additional Pages
- About page
- Contact form
- Admin dashboard
- Blog/News section (optional)

### Phase 6: Performance & SEO
- Image lazy loading
- Code splitting
- SEO optimization
- Performance monitoring
- Analytics integration

### Phase 7: Responsive Design & Testing
- Mobile responsiveness
- Cross-browser testing
- Performance optimization
- Security testing

### Phase 8: Deployment & Production
- Docker containerization
- CI/CD pipeline
- SSL certificates
- Domain configuration
- Monitoring setup

## Key Features to Implement

### Frontend Features
- [x] Hero section with background image
- [x] Portfolio galleries with categories
- [x] Image lightbox/modal
- [x] Responsive navigation
- [x] Contact form
- [x] Admin dashboard
- [x] User authentication UI
- [x] Image upload interface
- [x] SEO optimization
- [x] Loading states

### Backend Enhancements
- [x] File upload endpoints
- [x] Image processing/resizing
- [x] Portfolio section management
- [x] Contact form handler
- [x] Site settings API
- [x] SEO metadata endpoints

## Design Specifications

### Color Palette
- Primary: Dark theme (#000000, #1a1a1a)
- Secondary: White/Light gray (#ffffff, #f5f5f5)
- Accent: Gold/Bronze (#d4af37) - for buttons/highlights

### Typography
- Primary: Modern sans-serif (Inter, Poppins)
- Headers: Bold, uppercase for impact
- Body: Clean, readable spacing

### Layout Principles
- Full-screen hero sections
- Generous whitespace
- Grid-based portfolio layouts
- Smooth animations/transitions
- Mobile-first responsive design

## Development Timeline (Estimated)

**Week 1-2:** Frontend setup, hero section, basic routing
**Week 3-4:** Portfolio galleries, backend integration
**Week 5-6:** Additional pages, admin functionality
**Week 7-8:** Responsive design, testing, deployment

## Next Steps
1. Setup Vue.js project structure
2. Configure development environment
3. Create hero section component
4. Integrate with existing backend APIs
5. Build portfolio gallery system
6. Implement responsive design
7. Add admin functionality
8. Deploy and test

---
*This plan provides a roadmap for building a professional portfolio website similar to Rishi Raj Studio while leveraging your existing ASP.NET Core backend infrastructure.*
