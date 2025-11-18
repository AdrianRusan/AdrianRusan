'use client'

import { useState } from 'react'

export default function MapPage() {
  const [selectedParcel, setSelectedParcel] = useState<any>(null)

  return (
    <div className="h-screen flex flex-col">
      <header className="bg-white shadow-sm p-4">
        <h1 className="text-2xl font-bold">Map Viewer</h1>
      </header>

      <div className="flex-1 relative">
        {/* Map placeholder */}
        <div className="absolute inset-0 bg-gray-100 flex items-center justify-center">
          <div className="text-center">
            <p className="text-xl text-gray-600 mb-2">🗺️ Map Component</p>
            <p className="text-sm text-gray-500">MapLibre GL integration goes here</p>
            <p className="text-sm text-gray-500 mt-4">
              This will display parcels, applications, issues, and sales<br/>
              with interactive popups and layer controls
            </p>
          </div>
        </div>

        {/* Layer controls */}
        <div className="absolute top-4 right-4 bg-white rounded-lg shadow-lg p-4 max-w-xs">
          <h3 className="font-semibold mb-3">Layers</h3>
          <div className="space-y-2">
            <label className="flex items-center">
              <input type="checkbox" defaultChecked className="mr-2" />
              <span>Parcels</span>
            </label>
            <label className="flex items-center">
              <input type="checkbox" defaultChecked className="mr-2" />
              <span>Applications</span>
            </label>
            <label className="flex items-center">
              <input type="checkbox" defaultChecked className="mr-2" />
              <span>Issues</span>
            </label>
            <label className="flex items-center">
              <input type="checkbox" defaultChecked className="mr-2" />
              <span>Sales</span>
            </label>
          </div>
        </div>

        {/* Search box */}
        <div className="absolute top-4 left-4 bg-white rounded-lg shadow-lg p-2 w-96">
          <input
            type="text"
            placeholder="Search by parcel ID or address..."
            className="w-full px-4 py-2 border border-gray-300 rounded"
          />
        </div>
      </div>
    </div>
  )
}
