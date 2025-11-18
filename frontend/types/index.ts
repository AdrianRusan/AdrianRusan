// GeoJSON types
export interface GeoJsonGeometry {
  type: string
  coordinates: any
}

export interface GeoJsonFeature {
  type: 'Feature'
  geometry: GeoJsonGeometry | null
  properties: Record<string, any>
  id?: string
}

export interface GeoJsonFeatureCollection {
  type: 'FeatureCollection'
  features: GeoJsonFeature[]
  crs?: {
    type: string
    properties: {
      name: string
    }
  }
}

// Parcel types
export interface Parcel {
  id: string
  parcelId: string
  surfaceM2: number
  surfaceHa: number
  plotType: string
  geometry: GeoJsonFeature | null
  properties: Record<string, any>
}

// Application types
export interface Application {
  id: string
  applicationType: string
  status: string
  trackingNumber: string
  geom: GeoJsonFeature | null
  data: Record<string, any>
  createdAt: string
  updatedAt: string
  attachmentUrls: string[]
}

export interface ApplicationDetail extends Application {
  applicant?: {
    id: string
    email: string
    firstName: string
    lastName: string
    phone?: string
  }
  comments: Comment[]
}

export interface Comment {
  id: string
  comment: string
  authorName: string
  isInternal: boolean
  createdAt: string
}

// Issue types
export interface Issue {
  id: string
  title: string
  description: string
  geom: GeoJsonFeature | null
  status: string
  createdAt: string
  response?: string
  respondedAt?: string
}

// Sale types
export interface Sale {
  id: string
  userId: string
  parcelId?: string
  geom: GeoJsonFeature | null
  price: number
  currency: string
  description: string
  createdAt: string
  expiresAt: string
  isActive: boolean
  photoUrls: string[]
}

// Auth types
export interface AuthResponse {
  accessToken: string
  refreshToken: string
  email: string
  firstName: string
  lastName: string
  role: string
}

export interface RegisterRequest {
  email: string
  password: string
  firstName: string
  lastName: string
  phone?: string
}

export interface LoginRequest {
  email: string
  password: string
}

// Pagination
export interface PagedResult<T> {
  items: T[]
  page: number
  pageSize: number
  totalCount: number
  totalPages: number
}
