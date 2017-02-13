import { Component, OnInit } from "@angular/core";
import { MdlDefaultTableModel, IMdlTableModelItem } from "angular2-mdl";

import { HomeService } from "../home.service";

@Component({
    selector: "top-users",
    template: require("./top-users.component.html"),
    providers: [HomeService]
})
export class TopUsersComponent implements OnInit {
    usersLoaded: boolean = false;

    users = new MdlDefaultTableModel([
        {key:"userName", name: "Имя пользователя"},
        {key:"rating", name: "Рейтинг", numeric:true},
        {key:"solvedChallenges", name: "Решено задач", numeric:true},
        {key:"postedChallenges", name: "Создано задач", numeric:true}
    ]);

    constructor(private homeService: HomeService) { }

    ngOnInit(): void {
        this.homeService.getTopUsers().subscribe(users => {
            this.users.addAll(users as [IMdlTableModelItem]);
            this.usersLoaded = true;
        });
    }
}
