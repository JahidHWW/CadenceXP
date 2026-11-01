export type UserQuest = {
    questId: number,
    name: string,
    description: string,
    type: string,
    difficulty: string,
    requiredLevel: number,
    targetValue: number,
    xpReward: number,
    isUnlocked: boolean,
    unlockedAtUtc: string | null,
    progress: number,
    isCompleted: boolean,
    completedAtUtc: string | null
}