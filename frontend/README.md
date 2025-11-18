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
│   ├── Map/              # Map components
│   ├── forms/            # Form components
│   └── ui/               # UI components
├── services/             # API services
│   └── api.ts            # Axios configuration
├── hooks/                # Custom React hooks
├── types/                # TypeScript type definitions
├── lib/                  # Utility functions
└── public/               # Static assets
```

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
- Custom color palette
- Responsive design utilities
- Dark mode support (future)

## TODO

- [ ] Implement MapLibre GL map component
- [ ] Add geometry drawing tools
- [ ] Implement all application forms
- [ ] Add image upload and preview
- [ ] Implement admin dashboard
- [ ] Add pagination components
- [ ] Implement filtering and sorting
- [ ] Add loading states and skeletons
- [ ] Implement error boundaries
- [ ] Add E2E tests with Playwright
- [ ] Implement PWA features
- [ ] Add dark mode
