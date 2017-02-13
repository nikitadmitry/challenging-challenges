import { Component } from "@angular/core";

import { ChallengesComponent } from "./challenges.component";
import { SortingType } from "./models/SortingType";
import { HomeService } from "../home.service";

@Component({
    selector: "unsolved-challenges",
    template: require("./challenges.component.html"),
    styles: [require("./challenges.component.css")],
    providers: [HomeService]
})
export class UnsolvedChallengesComponent extends ChallengesComponent {
    protected sortingType: SortingType = SortingType.Unsolved;
    protected componentTitle: string = "Нерешенные задачи";
    protected paginatorId: string = "unsolved-challenges-paginator";

    constructor(homeService: HomeService) {
        super(homeService);
    }
}