export type UserAchievement = {
    achievementId: number,
    name: string,
    description: string,
    type: string,
    targetValue: number,
    xpReward: number,
    isEarned: boolean,
    earnedAtUtc: string | null,
    progress: number
}