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
    saveChallenge: string;
}

interface User {
    getUserById: string;
    setAbout: string;
}

declare module actions {
    var account: Account;
    var home: Home;
    var challenges: Challenges;
    var user: User;
}

export var Actions = actions;