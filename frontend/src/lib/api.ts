import type { Ride } from "@/types/ride";

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