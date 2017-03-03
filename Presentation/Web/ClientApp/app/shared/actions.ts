interface Account {
    login: string;
    register: string;
    checkUsernameAvailability: string;
    checkEmailAvailability: string;
    getUserName: string;
}

interface Home {
    getChallenges: string;
    getChallengesCount: string;
    getTopUsers: string;
    getPopularTags: string;
}

interface Challenges {
    searchChallenges: string;
    getChallenge: string;
    solve: string;
    getSourceCodeTemplate: string;
}

declare module actions {
    var account: Account;
    var home: Home;
    var challenges: Challenges;
}

export var Actions = actions;