import type { Ride } from "@/types/ride";
import type { TrackPoint } from "@/types/trackPoint";
import type { Bike } from "@/types/bike";

export async function getRides(): Promise<Ride[]> {
    const response = await fetch(
        `${process.env.NEXT_PUBLIC_API_URL}/api/rides`
    );

    if (!response.ok) {
        throw new Error("Failed to fetch rides.");
    }

    const rides: Ride[] = await response.json();

    return rides;
}

export async function getRideById(id: number): Promise<Ride> {
    const response = await fetch(
        `${process.env.NEXT_PUBLIC_API_URL}/api/rides/${id}`
    );

    if (!response.ok) {
        throw new Error("Failed to fetch ride.");
    }

    const ride: Ride = await response.json();

    return ride;
}

export async function getRideTrackPoints(
    id: number
): Promise<TrackPoint[]> {
    const response = await fetch(
        `${process.env.NEXT_PUBLIC_API_URL}/api/rides/${id}/track-points`
    );

    if (!response.ok) {
        throw new Error("Failed to fetch ride track points.");
    }

    const trackPoints: TrackPoint[] =
        await response.json();

    return trackPoints;
}

export async function uploadRide(
    formData: FormData
): Promise<Ride> {
    const response = await fetch(
        `${process.env.NEXT_PUBLIC_API_URL}/api/rides/upload`,
        {
            method: "POST",
            body: formData
        }
    );

    if (!response.ok) {
        const message = await response.text();

        throw new Error(
            message || "Failed to upload ride."
        );
    }

    const ride: Ride = await response.json();
    return ride;
}

export async function getUserBikes(
    userId: number
): Promise<Bike[]> {
    const url =
        `${process.env.NEXT_PUBLIC_API_URL}/api/users/${userId}/bikes`;

    const response = await fetch(url);

    if (!response.ok) {
        const message = await response.text();

        throw new Error(
            `Failed to fetch bikes: ${response.status} ${message}`
        );
    }

    const bikes: Bike[] = await response.json();

    return bikes;
}