import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { BookmarkDialogService, BordersAndShadingDialogService, BulletsAndNumberingDialogService, CellOptionsDialogService, ContextMenuService, DocumentEditorAllModule, DocumentEditorContainerModule, DocumentEditorModule, EditorHistoryService, EditorService, FontDialogService, HyperlinkDialogService, ImageResizerService, ListDialogService, OptionsPaneService, PageSetupDialogService, ParagraphDialogService, PrintService, SearchService, SelectionService, SfdtExportService, StyleDialogService, StylesDialogService, TableDialogService, TableOfContentsDialogService, TableOptionsDialogService, TablePropertiesDialogService, TextExportService, ToolbarService, WordExportService } from '@syncfusion/ej2-angular-documenteditor';
import { registerLicense } from '@syncfusion/ej2-base';
import { LICENSE } from './license';

registerLicense(LICENSE);

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    DocumentEditorContainerModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
