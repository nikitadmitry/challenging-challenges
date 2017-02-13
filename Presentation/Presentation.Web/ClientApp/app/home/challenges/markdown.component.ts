import { Component, Input, OnChanges } from "@angular/core";
import * as marked from "marked";

@Component({
  selector: "markdown",
  template: `
    <div [innerHTML]="convertedData">
    </div>
  `
})
export class MarkdownComponent implements OnChanges {
  @Input("data")
  data: string;

  convertedData: string;

  ngOnChanges(): void {
    var md: any = marked.setOptions({});
    this.convertedData = md.parse(this.data);
  }
}