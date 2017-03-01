import {Pipe, PipeTransform, NgModule} from '@angular/core';

@Pipe({name: 'enum'})
export class EnumPipe implements PipeTransform {
    transform(value: number, enumName: string): string {
        return `${enumName}.${enumName}-${value}`;
    }
}

@NgModule({
    declarations: [
        EnumPipe
    ],
    exports: [
        EnumPipe
    ]
})
export class EnumPipeModule { }