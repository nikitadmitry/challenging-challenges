interface Account {
    login: string;
    register: string;
    checkUsernameAvailability: (string) => string;
    checkEmailAvailability: (string) => string;
}

declare module actions {
    var account: Account;
}

export var Actions = actions;