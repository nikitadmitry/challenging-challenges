import { Component, ViewChild, OnInit } from "@angular/core";
import { TranslationService } from "angular-l10n";
import { DatatableComponent } from "@swimlane/ngx-datatable";

import { HomeService } from "../home.service";

@Component({
    selector: "top-users",
    template: require("./top-users.component.html"),
    providers: [HomeService]
})
export class TopUsersComponent implements OnInit {
    usersLoaded: boolean = false;

    @ViewChild(DatatableComponent)
    usersTable : DatatableComponent;

    constructor(private homeService: HomeService, private translation: TranslationService) { }

    ngOnInit(): void {
        this.translation.translationChanged.subscribe(() => {
            this.initializeTableColumns();
        });

        this.homeService.getTopUsers().subscribe(users => {
            this.initUsersTable(users);
            this.usersLoaded = true;
        });
    }

    private initUsersTable(users: any[]): void {
        this.initializeTableColumns();
        this.usersTable.rows = users;
        this.usersTable.sorts = [{prop: "rating", dir: "desc"}];
    }

    private initializeTableColumns(): void {
        this.usersTable.columns = [
            { prop: "userName", name: this.translation.translate("Home.UserName"), sortable: false },
            { prop: "rating", name: this.translation.translate("Home.Rating"), sortable: false },
            { prop: "solvedChallenges", name: this.translation.translate("Home.SolvedChallenges"), sortable: false },
            { prop: "postedChallenges", name: this.translation.translate("Home.PostedChallenges"), sortable: false }
        ];
    }

    openUserProfile(e: any): void {
        console.debug(e.row.userId);
    }
}
