'use client'

import { ReactNode } from 'react'

interface ParcelPopupProps {
  parcelId: string
  cadastralId?: string
  surfaceM2?: number
  surfaceHa?: number
  plotType?: string
  onRequestPermit?: () => void
  onReportIssue?: () => void
  onRequest3DScan?: () => void
  onViewSales?: () => void
  onClose: () => void
}

export default function ParcelPopup({
  parcelId,
  cadastralId,
  surfaceM2,
  surfaceHa,
  plotType,
  onRequestPermit,
  onReportIssue,
  onRequest3DScan,
  onViewSales,
  onClose,
}: ParcelPopupProps) {
  const hasData = cadastralId || surfaceM2 !== undefined

  return (
    <div className="bg-white rounded-lg shadow-xl max-w-sm w-full overflow-hidden">
      {/* Header */}
      <div className="bg-primary-600 text-white px-4 py-3 flex items-center justify-between">
        <h3 className="font-semibold text-lg">Parcel Details</h3>
        <button
          onClick={onClose}
          className="p-1 hover:bg-primary-700 rounded transition-colors"
          aria-label="Close"
        >
          <svg className="w-5 h-5" fill="none" strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" viewBox="0 0 24 24" stroke="currentColor">
            <path d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>
      </div>

      {/* Content */}
      <div className="p-4">
        {hasData ? (
          <div className="space-y-3">
            {/* Parcel Info */}
            <div className="grid grid-cols-2 gap-3 text-sm">
              {cadastralId && (
                <div>
                  <div className="text-gray-500 text-xs uppercase">Cadastral ID</div>
                  <div className="font-medium text-gray-900">{cadastralId}</div>
                </div>
              )}
              <div>
                <div className="text-gray-500 text-xs uppercase">Parcel ID</div>
                <div className="font-medium text-gray-900">{parcelId}</div>
              </div>
              {surfaceM2 !== undefined && (
                <>
                  <div>
                    <div className="text-gray-500 text-xs uppercase">Surface (m²)</div>
                    <div className="font-medium text-gray-900">{surfaceM2.toLocaleString()}</div>
                  </div>
                  <div>
                    <div className="text-gray-500 text-xs uppercase">Surface (ha)</div>
                    <div className="font-medium text-gray-900">{surfaceHa?.toFixed(4)}</div>
                  </div>
                </>
              )}
              {plotType && (
                <div className="col-span-2">
                  <div className="text-gray-500 text-xs uppercase">Type</div>
                  <div className="font-medium text-gray-900">{plotType}</div>
                </div>
              )}
            </div>

            {/* Action Buttons */}
            <div className="border-t pt-4 mt-4 space-y-2">
              <button
                onClick={onRequestPermit}
                className="w-full px-4 py-2.5 bg-primary-600 text-white rounded-lg hover:bg-primary-700 transition-colors font-medium text-sm flex items-center justify-center space-x-2"
              >
                <svg className="w-5 h-5" fill="none" strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" viewBox="0 0 24 24" stroke="currentColor">
                  <path d="M9 12h6m-6 4h6m2 5H7a2 2 0 01-2-2V5a2 2 0 012-2h5.586a1 1 0 01.707.293l5.414 5.414a1 1 0 01.293.707V19a2 2 0 01-2 2z" />
                </svg>
                <span>Request Permit</span>
              </button>

              <div className="grid grid-cols-3 gap-2">
                <button
                  onClick={onReportIssue}
                  className="px-3 py-2 bg-gray-100 text-gray-700 rounded-lg hover:bg-gray-200 transition-colors text-xs font-medium"
                >
                  🚧 Issue
                </button>
                <button
                  onClick={onRequest3DScan}
                  className="px-3 py-2 bg-gray-100 text-gray-700 rounded-lg hover:bg-gray-200 transition-colors text-xs font-medium"
                >
                  📐 3D Scan
                </button>
                <button
                  onClick={onViewSales}
                  className="px-3 py-2 bg-gray-100 text-gray-700 rounded-lg hover:bg-gray-200 transition-colors text-xs font-medium"
                >
                  🏘️ Sales
                </button>
              </div>
            </div>
          </div>
        ) : (
          <div className="text-center py-6">
            <div className="text-gray-400 mb-3">
              <svg className="w-16 h-16 mx-auto" fill="none" strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" viewBox="0 0 24 24" stroke="currentColor">
                <path d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
              </svg>
            </div>
            <p className="text-gray-600 font-medium mb-2">Data Unavailable</p>
            <p className="text-sm text-gray-500 mb-4">Some parcel information is missing</p>
            <button
              onClick={onReportIssue}
              className="text-primary-600 hover:text-primary-700 text-sm font-medium underline"
            >
              Report Missing Parcel Data
            </button>
          </div>
        )}
      </div>
    </div>
  )
}
