//#define AMAZON_CIRCLE_ENABLED
using UnityEngine;
using System.Collections;

public enum GC_ScoreTimeSpan {
	TODAY,
	WEEK,
	ALL_TIME
}

static class GC_ScoreTimeSpanExtensions {
#if AMAZON_CIRCLE_ENABLED
	public static LeaderboardScope GetLeaderboardScope(this GC_ScoreTimeSpan timeSpan) {
		switch (timeSpan) {
		case GC_ScoreTimeSpan.ALL_TIME : return LeaderboardScope.GlobalAllTime;
		case GC_ScoreTimeSpan.WEEK : return LeaderboardScope.GlobalWeek;
		case GC_ScoreTimeSpan.TODAY : return LeaderboardScope.GlobalDay;
		default: return LeaderboardScope.GlobalAllTime;
		}
	}
#endif
}

#if AMAZON_CIRCLE_ENABLED
static class LeaderboardsScopeExtensions {
	public static GC_ScoreTimeSpan GetGCTimeSpan(this LeaderboardScope scope) {
		switch (scope) {
		case LeaderboardScope.GlobalAllTime: return GC_ScoreTimeSpan.ALL_TIME;
		case LeaderboardScope.GlobalWeek: return GC_ScoreTimeSpan.WEEK;
		case LeaderboardScope.GlobalDay: return GC_ScoreTimeSpan.TODAY;
		default: return GC_ScoreTimeSpan.ALL_TIME;
		}
	}
}
#endif
