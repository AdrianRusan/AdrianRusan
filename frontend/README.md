# PUG Platform Frontend

Next.js 14 frontend application for the Local Council PUG Platform MVP.

## Tech Stack

- Next.js 14 with App Router
- TypeScript
- TailwindCSS for styling
- MapLibre GL for maps
- React Hook Form for forms
- TanStack Query for data fetching
- Axios for API calls
- next-i18next for internationalization (RO/EN/HU)

## Prerequisites

- Node.js 20+
- npm or yarn

## Installation

```bash
npm install
```

## Development

```bash
npm run dev
```

Open [http://localhost:3000](http://localhost:3000) in your browser.

## Environment Variables

Create a `.env.local` file:

```env
NEXT_PUBLIC_API_URL=http://localhost:5000
```

## Building

```bash
npm run build
npm start
```

## Docker

```bash
docker build -t pug-platform-frontend .
docker run -p 3000:3000 pug-platform-frontend
```

## Project Structure

```
frontend/
├── app/                    # Next.js App Router pages
│   ├── layout.tsx         # Root layout
│   ├── page.tsx           # Home page
│   ├── map/               # Map viewer
│   ├── applications/      # Applications pages
│   ├── issues/            # Issues pages
│   ├── marketplace/       # Sales marketplace
│   └── admin/             # Admin dashboard
├── components/            # React components
│   ├── Layout/           # Layout components (Header, Layout)
│   ├── Map/              # Map components (ParcelPopup, MapControlPanel)
│   ├── Forms/            # Form components (MultiStepForm)
│   ├── Admin/            # Admin components (AdminLayout)
│   └── UI/               # UI components (FAB, Toast, LoadingSpinner)
├── services/             # API services
│   └── api.ts            # Axios configuration
├── hooks/                # Custom React hooks
│   ├── useMediaQuery.ts  # Responsive breakpoint hooks
│   └── useTouchGestures.ts # Touch gesture support
├── types/                # TypeScript type definitions
├── lib/                  # Utility functions
└── public/               # Static assets
    └── locales/          # i18n translation files (ro/en/hu)
```

## 🎨 Design System (Mobile-First)

### Design Principles (Part 4 Implementation)
- **Mobile-First:** All components designed for 320px-768px first, then enhanced for desktop
- **Touch-Friendly:** Minimum 44x44px tap targets
- **Responsive:** Tailwind breakpoints (sm:640, md:768, lg:1024, xl:1280)
- **Accessible:** ARIA labels, keyboard navigation, high contrast
- **Performant:** CSS transitions, lazy loading ready

### Color Palette
```css
Primary:   #0D6EFD (bg-primary-600)  - Main actions, headers
Secondary: #6C757D (bg-gray-600)     - Secondary elements
Success:   #28A745 (bg-green-600)    - Completed states
Warning:   #FFC107 (bg-yellow-500)   - Warnings
Danger:    #DC3545 (bg-red-600)      - Errors, deletions
```

### Component Library

#### Layout Components ✅
- **Header** (`components/Layout/Header.tsx`)
  - Fixed top navigation
  - Language selector (RO/EN/HU)
  - Responsive menu (hamburger on mobile)
  - Active route highlighting

- **Layout** (`components/Layout/Layout.tsx`)
  - Page wrapper with consistent spacing
  - Optional header display

- **AdminLayout** (`components/Admin/AdminLayout.tsx`)
  - Collapsible sidebar (overlay on mobile, fixed on desktop)
  - Menu with 8 admin sections
  - Responsive navigation

#### UI Components ✅
- **FloatingActionButton** (`components/UI/FloatingActionButton.tsx`)
  - Bottom-right FAB (56px circle)
  - Expandable menu with labeled actions
  - Smooth animations

- **Toast** (`components/UI/Toast.tsx`)
  - Success/Error/Warning/Info notifications
  - Auto-dismiss (3s default)
  - Top-right position

- **LoadingSpinner** (`components/UI/LoadingSpinner.tsx`)
  - 3 sizes: sm/md/lg
  - 3 colors: primary/white/gray

#### Form Components ✅
- **MultiStepForm** (`components/Forms/MultiStepForm.tsx`)
  - Numbered progress bar
  - Previous/Next/Submit navigation
  - Loading states
  - Responsive step indicators
  - Mobile: Vertical step title
  - Desktop: Horizontal with all titles visible

#### Map Components ✅
- **ParcelPopup** (`components/Map/ParcelPopup.tsx`)
  - Displays parcel details (ID, surface m²/ha, type)
  - 4 action buttons:
    - Request Permit (primary)
    - Report Issue (secondary)
    - Request 3D Scan (secondary)
    - View Sales (secondary)
  - Fallback for missing data with "Report" link

- **MapControlPanel** (`components/Map/MapControlPanel.tsx`)
  - Layer toggles with checkboxes
  - Search bar
  - Legend with color indicators
  - Responsive:
    - Mobile: Bottom sheet with toggle handle
    - Desktop: Right panel (384px width)

### Custom Hooks ✅
- **useMediaQuery** (`hooks/useMediaQuery.ts`)
  - Responsive design hooks
  - `useIsMobile()`, `useIsTablet()`, `useIsDesktop()`
  - `useBreakpoint()` - returns 'mobile'|'tablet'|'desktop'

- **useTouchGestures** (`hooks/useTouchGestures.ts`)
  - Swipe detection (left/right/up/down)
  - Long press support
  - Touch event handlers

## Features

### Public Features
- View interactive map with parcels
- Browse marketplace for land sales
- Report issues without registration
- View parcel details

### Authenticated User Features
- Submit applications (tree cutting, demolition, construction)
- Track application status
- Create and manage sale listings
- Upload photos and documents

### Staff Features
- Review and process applications
- Respond to issues
- Change application statuses
- View admin dashboard

## Map Integration

The map uses MapLibre GL with the following layers:
- Parcels (polygons)
- Applications (points/polygons)
- Issues (points)
- Sales (polygons)

Clicking on features opens popups with detailed information.

## Forms

All forms use:
- React Hook Form for form state management
- Client-side validation
- Server-side error handling
- File upload support
- Geometry drawing on map

## Internationalization

The app supports three languages:
- Romanian (ro) - default
- English (en)
- Hungarian (hu)

Language can be switched using the language selector in the header.

## API Integration

The frontend communicates with the backend API using Axios. Authentication tokens are stored in localStorage and automatically attached to requests.

Refresh tokens are stored in httpOnly cookies and automatically used when access tokens expire.

## Styling

TailwindCSS is used for all styling with a custom configuration that includes:
- Custom color palette (Primary: #0D6EFD)
- Responsive design utilities
- Mobile-first approach
- Touch-friendly sizing (min 44px tap targets)
- Smooth transitions (150ms/300ms)
- Shadow system (md/lg/xl)

### Responsive Breakpoints
```css
sm:  640px  - Small tablets
md:  768px  - Tablets
lg:  1024px - Small desktops
xl:  1280px - Large desktops
2xl: 1536px - Extra large screens
```

### Component Usage Examples

**Header with Language Selector:**
```tsx
import Header from '@/components/Layout/Header'

<Header
  currentLanguage="ro"
  onLanguageChange={(lang) => console.log(lang)}
/>
```

**Floating Action Button:**
```tsx
import FloatingActionButton from '@/components/UI/FloatingActionButton'

<FloatingActionButton
  items={[
    { icon: '🌲', label: 'Tree Cutting', onClick: () => {} },
    { icon: '🏗️', label: 'Demolition', onClick: () => {} },
    { icon: '🚧', label: 'Report Issue', onClick: () => {} },
  ]}
/>
```

**Multi-Step Form:**
```tsx
import MultiStepForm from '@/components/Forms/MultiStepForm'

const steps = [
  { title: 'Location', description: 'Select location' },
  { title: 'Details', description: 'Enter details' },
  { title: 'Review', description: 'Review and submit' },
]

<MultiStepForm
  steps={steps}
  currentStep={0}
  onNext={() => {}}
  onPrevious={() => {}}
  onSubmit={() => {}}
>
  {/* Step content */}
</MultiStepForm>
```

**Parcel Popup:**
```tsx
import ParcelPopup from '@/components/Map/ParcelPopup'

<ParcelPopup
  parcelId="BUC-001-2024"
  surfaceM2={1250.5}
  surfaceHa={0.125}
  plotType="Intravilan"
  onRequestPermit={() => {}}
  onReportIssue={() => {}}
  onRequest3DScan={() => {}}
  onViewSales={() => {}}
  onClose={() => {}}
/>
```

**Responsive Hooks:**
```tsx
import { useIsMobile, useIsDesktop } from '@/hooks/useMediaQuery'

const isMobile = useIsMobile()  // < 768px
const isDesktop = useIsDesktop()  // >= 1024px
```

**Touch Gestures:**
```tsx
import { useTouchGestures } from '@/hooks/useTouchGestures'

const gestures = useTouchGestures({
  onSwipeLeft: () => console.log('Swiped left'),
  onSwipeRight: () => console.log('Swiped right'),
  onLongPress: () => console.log('Long pressed'),
})

<div {...gestures}>Swipeable content</div>
```

## 📱 Mobile-First Development

All components follow mobile-first principles:
- Designed for touch interaction
- Minimum 44px tap targets
- Swipeable panels and cards
- Bottom sheets for mobile, side panels for desktop
- Collapsible menus and controls
- Optimized for slow connections

## 📚 Documentation

For complete design specifications, see:
- [PART4_FRONTEND_DESIGN.md](../PART4_FRONTEND_DESIGN.md) - Complete component documentation
- [Color system, breakpoints, accessibility guidelines](../PART4_FRONTEND_DESIGN.md#design-system-adherence)
- [Component usage examples](../PART4_FRONTEND_DESIGN.md#component-usage-examples)

## ✅ Implementation Status

### Completed (Part 4)
- ✅ Layout components (Header, Layout, AdminLayout)
- ✅ Floating Action Button with expandable menu
- ✅ Multi-step form with progress indicator
- ✅ Enhanced parcel popup with actions
- ✅ Map control panel (responsive)
- ✅ Toast notifications
- ✅ Loading spinner
- ✅ Responsive hooks (useMediaQuery, useBreakpoint)
- ✅ Touch gesture support (useTouchGestures)

## TODO

### High Priority
- [ ] Implement MapLibre GL map integration
- [ ] Add geometry drawing tools
- [ ] Implement application forms (TreeCutting, Demolition, Construction)
- [ ] Add image upload with preview and compression
- [ ] Implement admin dashboard pages
- [ ] Add data tables with sorting/filtering
- [ ] Implement marketplace components

### Medium Priority
- [ ] Add pagination components
- [ ] Implement filtering and sorting UI
- [ ] Add loading skeletons
- [ ] Implement error boundaries
- [ ] Add statistics cards for dashboard
- [ ] Create form input library (validated inputs)

### Low Priority
- [ ] Add E2E tests with Playwright
- [ ] Implement PWA features
- [ ] Add dark mode toggle
- [ ] Implement offline detection
- [ ] Add service worker
