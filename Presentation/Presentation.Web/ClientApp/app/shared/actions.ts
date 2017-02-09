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
}

declare module actions {
    var account: Account;
    var home: Home;
}

export var Actions = actions;