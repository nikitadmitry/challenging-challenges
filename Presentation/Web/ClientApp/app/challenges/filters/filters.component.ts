import {Component, Input, Output, EventEmitter, ViewChild, AfterViewInit, OnDestroy} from "@angular/core";
import { Translation, TranslationService } from "angular-l10n";

import { ChallengeSearchType } from "../models/ChallengeSearchType";
import {MdlSelectComponent} from "@angular2-mdl-ext/select";
import {EnumSelectService} from "../../shared/services/enum-select.service";
import {Subject} from "rxjs";

@Component({
    selector: "filters",
    template: require("./filters.component.html"),
    styles: [require("./filters.component.css")],
    providers: [EnumSelectService]
})
export class FiltersComponent extends Translation implements AfterViewInit, OnDestroy {
    private ngUnsubscribe: Subject<void> = new Subject<void>();
    @Input() searchType: ChallengeSearchType;
    @Output() searchTypeChange: EventEmitter<ChallengeSearchType> = new EventEmitter<ChallengeSearchType>();

    @Input() searchString: string;
    @Output() searchStringChange: EventEmitter<string> = new EventEmitter<string>();

    searchTypes: any[];
    @Output() changeFilter: EventEmitter<void> = new EventEmitter<void>(true);
    @ViewChild("searchTypeSelect") searchTypeSelect: MdlSelectComponent;

    constructor(translationService: TranslationService, private enumSelectService: EnumSelectService) {
        super(translationService);

        this.translation.translationChanged.takeUntil(this.ngUnsubscribe)
            .subscribe(() => this.initializeSearchTypes());
    }

    ngAfterViewInit(): void {
        this.searchTypeSelect.writeValue(this.searchType);
    }

    ngOnDestroy() {
        this.ngUnsubscribe.next();
        this.ngUnsubscribe.complete();
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
        this.searchTypes = this.enumSelectService.convertToSelectValues(ChallengeSearchType);
    }
}