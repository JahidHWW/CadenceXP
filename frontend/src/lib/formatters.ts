export function metersToMiles(meters: number): number {
    return meters / 1609.344;
}

export function metersToFeet(meters: number): number {
    return meters * 3.28084;
}

export function formatDuration(seconds: number): string {
    const hours = Math.floor(seconds / 3600);
    const minutes = Math.floor((seconds % 3600) / 60);

    if (hours > 0) {
        return `${hours}h ${minutes}m`;
    }

    return `${minutes}m`;
}