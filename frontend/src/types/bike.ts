export type Bike = {
    id: number;
    name: string;
    type: string;
    userId: number;
}

export type CreateBikeRequest = {
    name: string;
    type: string;
    userId: number;
};