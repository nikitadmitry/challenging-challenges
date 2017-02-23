import {Component, OnInit} from "@angular/core";
import { TranslationService } from "angular-l10n";

import { HomeService } from "../home.service";

@Component({
    selector: "top-users",
    template: require("./top-users.component.html"),
    providers: [HomeService],
    styles: [`
        mdl-list-item {
            cursor: pointer;
        }
    `]
})
export class TopUsersComponent implements OnInit {
    usersLoaded: boolean = false;
    users: Array<any>;

    constructor(private homeService: HomeService, private translation: TranslationService) { }

    ngOnInit(): void {
        this.translation.translationChanged.subscribe(() => {
            this.initializeDescriptions();
        });

        this.homeService.getTopUsers().subscribe((users) => {
            this.users = users;
            this.initializeDescriptions();
            this.usersLoaded = true;
        });
    }

    initializeDescriptions() {
        this.users.forEach(user => this.setDescription(user));
    }

    openUserProfile(userId: string): void {
        console.debug(userId);
    }

    private setDescription(user: any) {
        user.description = this.translation.translate("Home.TopUserDescriptionTemplate",
            [user.solvedChallenges, user.postedChallenges,user.rating]);
    }
}
