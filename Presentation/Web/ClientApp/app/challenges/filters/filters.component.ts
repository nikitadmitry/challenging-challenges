import {Component, Input, Output, EventEmitter, ViewChild, AfterViewInit} from "@angular/core";
import { Translation, TranslationService } from "angular-l10n";

import { ChallengeSearchType } from "../models/ChallengeSearchType";
import {MdlSelectComponent} from "@angular2-mdl-ext/select";

@Component({
    selector: "filters",
    template: require("./filters.component.html"),
    styles: [require("./filters.component.css")]
})
export class FiltersComponent extends Translation implements AfterViewInit {
    @Input() searchType: ChallengeSearchType;
    @Output() searchTypeChange: EventEmitter<ChallengeSearchType> = new EventEmitter<ChallengeSearchType>();

    @Input() searchString: string;
    @Output() searchStringChange: EventEmitter<string> = new EventEmitter<string>();

    searchTypes: any[];
    @Output() changeFilter: EventEmitter<void> = new EventEmitter<void>(true);
    @ViewChild("searchTypeSelect") searchTypeSelect: MdlSelectComponent;

    constructor(translationService: TranslationService) {
        super(translationService);

        this.translation.translationChanged.subscribe(() => {
            this.initializeSearchTypes();
        });
    }

    ngAfterViewInit(): void {
        setTimeout(() => this.searchTypeSelect.writeValue(this.searchType), 500); // component is bugged first time.
    }

    submit(): void {
        this.searchTypeChange.emit(this.searchType);
        this.searchStringChange.emit(this.searchString);

        this.changeFilter.emit();
    }

    clear(): void {
        this.searchType = undefined;
        this.searchString = undefined;
        this.submit();
    }

    private initializeSearchTypes(): void {
        this.searchTypes = [
            {type: ChallengeSearchType.Title, name: this.translation.translate("Title")},
            {type: ChallengeSearchType.Condition, name: this.translation.translate("Condition")},
            {type: ChallengeSearchType.Difficulty, name: this.translation.translate("Difficulty")},
            {type: ChallengeSearchType.Language, name: this.translation.translate("Language")},
            {type: ChallengeSearchType.PreviewText, name: this.translation.translate("PreviewText")},
            {type: ChallengeSearchType.Section, name: this.translation.translate("Section")},
            {type: ChallengeSearchType.Tags, name: this.translation.translate("Tags")}
        ];
    }
}