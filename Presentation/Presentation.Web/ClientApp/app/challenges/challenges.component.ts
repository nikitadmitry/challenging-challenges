import { Component, OnInit, ViewChild } from "@angular/core";
import { MdlSelectComponent } from "@angular2-mdl-ext/select";
import { PaginationInstance } from "ng2-pagination";

import { ChallengesService } from "../challenges/challenges.service";
import { ChallengeSearchType } from "./models/ChallengeSearchType";
import { ChallengesPageRule } from "./models/ChallengesPageRule";

@Component({
    selector: "challenges",
    template: require("./challenges.component.html"),
    styles: [require("./challenges.component.css")],
    providers: [ChallengesService]
})
export class ChallengesComponent implements OnInit {
    private allType: number = ChallengeSearchType.All;
    private PAGE_SIZE: number = 10;
    selectedSearchTypes: ChallengeSearchType[];
    searchString: string;
    searchTypes = [
        {type: ChallengeSearchType.Condition, name: "Условие"},
        {type: ChallengeSearchType.Difficulty, name: "Сложность"},
        {type: ChallengeSearchType.Language, name: "Язык"},
        {type: ChallengeSearchType.PreviewText, name: "Текст Превью"},
        {type: ChallengeSearchType.Section, name: "Раздел"},
        {type: ChallengeSearchType.Tags, name: "Тэги"},
        {type: ChallengeSearchType.Title, name: "Название"}
    ];
    @ViewChild(MdlSelectComponent) searchTypeSelect: MdlSelectComponent;
    config: PaginationInstance = {
        id: "search-challenges-paginator",
        itemsPerPage: this.PAGE_SIZE,
        currentPage: 1
    };
    challenges: any[];
    isLoading: boolean = true;

    constructor(private challengesService: ChallengesService) { }

    ngOnInit(): void {
        this.setDefaultSearchType();

        this.challengesService.getChallengesCount()
            .subscribe(count => {
                this.config.totalItems = count;
                this.searchChallenges();
            });
    }

    onSearchTypesChange(): void {
        if (this.selectedSearchTypes.length === 0) { // empty -> add "All"
            this.setDefaultSearchType();
        } else {
            var lastAddedType: any = this.selectedSearchTypes[this.selectedSearchTypes.length - 1];

            if (lastAddedType === this.allType) { // if added "All", remove all other.
                this.setDefaultSearchType();
            } else if (this.searchTypes.filter(x => x.type !== this.allType).every(x => this.selectedSearchTypes.indexOf(x.type) > -1)
                || this.selectedSearchTypes.length === this.searchTypes.length) { // if selected all, remove all but "All"
                this.setDefaultSearchType();
            } else { // if added any other, remove "All"
                var allIndex: number = this.selectedSearchTypes.indexOf(this.allType);
                if (allIndex > -1) {
                    this.selectedSearchTypes.splice(allIndex, 1);
                }
            }
        }
        this.searchTypeSelect.ngModel = this.selectedSearchTypes;
        this.searchTypeSelect.writeValue(this.selectedSearchTypes);
    }

    private setDefaultSearchType(): void {
        this.selectedSearchTypes = [this.allType];
    }

    searchChallenges(): void {
        this.isLoading = true;
        this.challengesService.search(this.getPageRule()).subscribe(challenges => {
            this.challenges = challenges;
            this.isLoading = false;
        });
    }

    private getPageRule(): ChallengesPageRule {
        let pageRule: ChallengesPageRule = new ChallengesPageRule();
        pageRule.count = this.PAGE_SIZE;
        pageRule.start = (this.config.currentPage - 1) * this.PAGE_SIZE;
        pageRule.keyword = this.searchString;
        pageRule.searchTypes = this.selectedSearchTypes;

        return pageRule;
    }
}
