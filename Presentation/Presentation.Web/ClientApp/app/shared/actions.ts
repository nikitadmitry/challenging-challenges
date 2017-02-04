interface Account {
    login: string;
    register: string;
}

declare module actions {
    var account: Account;
}

export var Actions = actions;