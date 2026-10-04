"use client";

import { useEffect } from "react";

import {
    CircleMarker,
    MapContainer,
    Polyline,
    TileLayer,
    Tooltip,
    useMap
} from "react-leaflet";

import L from "leaflet";

import type { TrackPoint } from "@/types/trackPoint";

type RideMapProps = {
    trackPoints: TrackPoint[];
};

function MapRouteFitter({
    positions
}: {
    positions: [number, number][];
}) {
    const map = useMap();

    useEffect(() => {
        let secondFrame: number;

        const firstFrame = requestAnimationFrame(() => {
            secondFrame = requestAnimationFrame(() => {
                map.invalidateSize({
                    pan: false
                });

                if (positions.length > 1) {
                    const bounds = L.latLngBounds(positions);

                    map.fitBounds(bounds, {
                        padding: [30, 30]
                    });
                }
            });
        });

        const mapContainer = map.getContainer();

        const resizeObserver = new ResizeObserver(() => {
            map.invalidateSize({
                pan: false
            });
        });

        resizeObserver.observe(mapContainer);

        return () => {
            cancelAnimationFrame(firstFrame);

            if (secondFrame) {
                cancelAnimationFrame(secondFrame);
            }

            resizeObserver.disconnect();
        };
    }, [map, positions]);

    return null;
}

export default function RideMap({
    trackPoints
}: RideMapProps) {
    if (trackPoints.length === 0) {
        return (
            <div className="rounded-xl border border-zinc-800 bg-zinc-900 p-6 text-zinc-400">
                No route data available for this ride.
            </div>
        );
    }

    const positions: [number, number][] =
        trackPoints.map((point) => [
            point.latitude,
            point.longitude
        ]);

    const firstPoint = positions[0];
    const lastPoint = positions[positions.length - 1];

    return (
        <div className="h-[400px] w-full overflow-hidden rounded-xl">
            <MapContainer
                center={firstPoint}
                zoom={15}
                className="h-full w-full"
            >
                <MapRouteFitter positions={positions} />

                <TileLayer
                    attribution="&copy; OpenStreetMap contributors"
                    url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                />

                <Polyline positions={positions} />
                <Polyline positions={positions} />

                <CircleMarker
                    center={firstPoint}
                    radius={8}
                    pathOptions={{
                        color: "green",
                        fillColor: "green",
                        fillOpacity: 1
                    }}
                >
                    <Tooltip>Start</Tooltip>
                </CircleMarker>

                <CircleMarker
                    center={lastPoint}
                    radius={8}
                    pathOptions={{
                        color: "red",
                        fillColor: "red",
                        fillOpacity: 1
                    }}
                >
                    <Tooltip>Finish</Tooltip>
                </CircleMarker>
            </MapContainer>
        </div>
    );
}