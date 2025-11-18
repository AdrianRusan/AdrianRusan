'use client'

import { useState } from 'react'

interface LayerControl {
  id: string
  name: string
  icon: string
  enabled: boolean
}

interface MapControlPanelProps {
  layers: LayerControl[]
  onLayerToggle: (layerId: string) => void
  onSearch?: (query: string) => void
  position?: 'bottom' | 'right'
}

export default function MapControlPanel({
  layers,
  onLayerToggle,
  onSearch,
  position = 'bottom',
}: MapControlPanelProps) {
  const [searchQuery, setSearchQuery] = useState('')
  const [isExpanded, setIsExpanded] = useState(false)

  const handleSearch = (e: React.FormEvent) => {
    e.preventDefault()
    onSearch?.(searchQuery)
  }

  const containerClasses = position === 'bottom'
    ? 'fixed bottom-0 left-0 right-0 lg:left-auto lg:right-4 lg:bottom-4 lg:w-96'
    : 'fixed right-0 top-16 bottom-0 w-96 hidden lg:block'

  return (
    <div className={containerClasses}>
      <div className="bg-white rounded-t-2xl lg:rounded-2xl shadow-2xl overflow-hidden">
        {/* Mobile Toggle Handle */}
        <div className="lg:hidden bg-gray-100 py-2 flex justify-center">
          <button
            onClick={() => setIsExpanded(!isExpanded)}
            className="w-12 h-1.5 bg-gray-400 rounded-full"
            aria-label="Toggle panel"
          />
        </div>

        {/* Panel Content */}
        <div className={`${isExpanded || position === 'right' ? 'block' : 'hidden lg:block'}`}>
          {/* Search Bar */}
          {onSearch && (
            <div className="p-4 border-b border-gray-200">
              <form onSubmit={handleSearch}>
                <div className="relative">
                  <input
                    type="text"
                    value={searchQuery}
                    onChange={(e) => setSearchQuery(e.target.value)}
                    placeholder="Search by parcel ID or address..."
                    className="w-full pl-10 pr-4 py-2.5 border border-gray-300 rounded-lg focus:ring-2 focus:ring-primary-500 focus:border-transparent text-sm"
                  />
                  <svg
                    className="absolute left-3 top-3 w-5 h-5 text-gray-400"
                    fill="none"
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    strokeWidth="2"
                    viewBox="0 0 24 24"
                    stroke="currentColor"
                  >
                    <path d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z" />
                  </svg>
                </div>
              </form>
            </div>
          )}

          {/* Layer Controls */}
          <div className="p-4">
            <h3 className="text-sm font-semibold text-gray-900 mb-3 uppercase tracking-wide">
              Map Layers
            </h3>
            <div className="space-y-2">
              {layers.map((layer) => (
                <label
                  key={layer.id}
                  className="flex items-center p-3 rounded-lg hover:bg-gray-50 cursor-pointer transition-colors"
                >
                  <input
                    type="checkbox"
                    checked={layer.enabled}
                    onChange={() => onLayerToggle(layer.id)}
                    className="w-5 h-5 text-primary-600 border-gray-300 rounded focus:ring-primary-500"
                  />
                  <span className="ml-3 text-2xl">{layer.icon}</span>
                  <span className="ml-3 text-sm font-medium text-gray-700">
                    {layer.name}
                  </span>
                </label>
              ))}
            </div>
          </div>

          {/* Legend */}
          <div className="p-4 bg-gray-50 border-t border-gray-200">
            <h3 className="text-xs font-semibold text-gray-700 mb-2 uppercase tracking-wide">
              Legend
            </h3>
            <div className="space-y-2 text-xs">
              <div className="flex items-center space-x-2">
                <div className="w-4 h-4 bg-blue-500 border border-blue-700 rounded-sm"></div>
                <span className="text-gray-600">Parcels</span>
              </div>
              <div className="flex items-center space-x-2">
                <div className="w-4 h-4 bg-green-500 rounded-full"></div>
                <span className="text-gray-600">Issues</span>
              </div>
              <div className="flex items-center space-x-2">
                <div className="w-4 h-4 bg-yellow-500 rounded-full"></div>
                <span className="text-gray-600">Applications</span>
              </div>
              <div className="flex items-center space-x-2">
                <div className="w-4 h-4 bg-purple-500 rounded-full"></div>
                <span className="text-gray-600">Sales</span>
              </div>
              <div className="flex items-center space-x-2">
                <div className="w-4 h-4 bg-red-500 rounded-full"></div>
                <span className="text-gray-600">3D Scans</span>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}
