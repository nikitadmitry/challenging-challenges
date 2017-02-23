interface Account {
    login: string;
    register: string;
    checkUsernameAvailability: (string) => string;
    checkEmailAvailability: (string) => string;
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
}

declare module actions {
    var account: Account;
    var home: Home;
    var challenges: Challenges;
}

export var Actions = actions;