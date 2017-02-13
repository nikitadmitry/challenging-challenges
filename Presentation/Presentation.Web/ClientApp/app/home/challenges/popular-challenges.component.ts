import { Component } from "@angular/core";

import { ChallengesComponent } from "./challenges.component";
import { SortingType } from "./models/SortingType";
import { HomeService } from "../home.service";

@Component({
    selector: "popular-challenges",
    template: require("./challenges.component.html"),
    styles: [require("./challenges.component.css")],
    providers: [HomeService]
})
export class PopularChallengesComponent extends ChallengesComponent {
    protected sortingType: SortingType = SortingType.Popular;
    protected componentTitle: string = "Полулярные задачи";
    protected paginatorId: string = "popular-challenges-paginator";

    constructor(homeService: HomeService) {
        super(homeService);
    }
}