export type Ride = {
    id: number;
    name: string;

    userId: number;
    userDisplayName: string;

    bikeId: number;
    bikeName: string;

    distanceMeters: number;
    elevationGainMeters: number;
    durationSeconds: number;

    rideDateUtc: string;

    xpEarned: number;

    originalFileName: string | null;

    processingStatus:
    | "Pending"
    | "Processing"
    | "Completed"
    | "Failed";
};