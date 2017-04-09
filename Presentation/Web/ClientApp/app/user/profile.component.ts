import { Component, OnInit } from '@angular/core';
import {ActivatedRoute, Router} from "@angular/router";
import {UserService} from "./user.service";
import * as $ from "jquery";
require("jquery-knob");

@Component({
    selector: 'profile',
    template: require("./profile.component.html"),
    styles: [require("./profile.component.css")],
    providers: [UserService]
})
export class ProfileComponent implements OnInit {
    model: UserModel;
    constructor(route: ActivatedRoute, private userService: UserService) {
        route.params.subscribe((params) => {
            this.userService.getUser(params["id"]).subscribe((model) => {
                this.model = model;
                setTimeout(() => this.initKnobs(), 0);
            });
        });
    }

    ngOnInit() {

    }

    initKnobs() {
        (<any>$("#user-level")).knob({
            'angleOffset': -125,
            'angleArc': 250,
            'readOnly': true
        });

        (<any>$("#user-solvedChallenges, #user-postedChallenges")).knob({
            'fgColor': '#66CC66',
            'thickness': '.2',
            'angleOffset': 180,
            'readOnly': true
        });
    }

}