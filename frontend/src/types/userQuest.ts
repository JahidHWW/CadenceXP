export type UserQuest = {
    questId: number,
    name: string,
    description: string,
    type: string,
    targetValue: number,
    xpReward: number,
    progress: number,
    isCompleted: boolean,
    completedAtUtc: string | null
}