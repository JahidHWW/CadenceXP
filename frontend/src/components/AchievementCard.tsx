import type { UserAchievement } from "@/types/userAchievement";

type AchievementCardProps = {
    achievement: UserAchievement;
};

export default function AchievementCard({
    achievement
}: AchievementCardProps) {
    const progressPercent =
        achievement.targetValue > 0
            ? Math.min(
                100,
                (achievement.progress / achievement.targetValue) * 100
            )
            : 0;

    let progressLabel;

    switch (achievement.type) {
        case "TotalDistance":
        case "SingleRideDistance":
            progressLabel =
                `${achievement.progress.toFixed(1)} / ${achievement.targetValue.toFixed(1)} mi`;
            break;

        case "TotalElevation":
            progressLabel =
                `${achievement.progress.toFixed(0)} / ${achievement.targetValue.toFixed(0)} m`;
            break;

        case "RideCount":
            progressLabel =
                `${achievement.progress.toFixed(0)} / ${achievement.targetValue.toFixed(0)} rides`;
            break;

        default:
            progressLabel =
                `${achievement.progress} / ${achievement.targetValue}`;
    }

    return (
        <div
            className={`rounded-xl border p-5 ${achievement.isEarned
                ? "border-lime-900 bg-zinc-900"
                : "border-zinc-800 bg-zinc-900"
                }`}
        >
            <div className="flex items-start justify-between gap-4">
                <div>
                    <h3 className="text-lg font-semibold">
                        {achievement.name}
                    </h3>

                    <p className="mt-1 text-sm text-zinc-400">
                        {achievement.description}
                    </p>
                </div>

                <p className="font-semibold text-lime-400">
                    +{achievement.xpReward} XP
                </p>
            </div>

            {achievement.isEarned ? (
                <p className="mt-4 font-medium text-lime-400">
                    Earned
                </p>
            ) : (
                <div className="mt-5">
                    <div className="mb-2 flex items-center justify-between">
                        <span className="text-sm text-zinc-300">
                            Progress
                        </span>

                        <span className="text-sm text-zinc-400">
                            {progressLabel}
                        </span>
                    </div>

                    <div className="h-3 w-full overflow-hidden rounded-full bg-zinc-800">
                        <div
                            className="h-full rounded-full bg-lime-400"
                            style={{
                                width: `${progressPercent}%`
                            }}
                        />
                    </div>
                </div>
            )}
        </div>
    );
}