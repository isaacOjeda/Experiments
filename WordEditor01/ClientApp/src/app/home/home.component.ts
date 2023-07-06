import { Component, OnInit, ViewChild } from '@angular/core';
import { DocumentEditorContainerComponent, ToolbarItem } from '@syncfusion/ej2-angular-documenteditor';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  @ViewChild('documentEditor')
  public documentEditor?: DocumentEditorContainerComponent;


  public onCreate() {
    console.log('created');

  }

  public print() {
    console.log('print');
    this.documentEditor!.documentEditor.print();


  }

  public download() {
    console.log('download');
    this.documentEditor!.documentEditor.save('Sample', 'Docx');
  }

  public saveAsBlobAndUploadToServer() {
    console.log('saveAsBlobAndUploadToServer');
    this.documentEditor!.documentEditor.saveAsBlob('Docx').then((blob) => {

      console.log('blob', blob);

      const formData = new FormData();
      formData.append('file', blob, 'Sample.docx');
      fetch('upload', {
        method: 'POST',
        body: formData
      });
    });
  }

  public openFileFromServer() {
    console.log('openFileFromServer');
    fetch('demo', {
      method: 'GET'
    }).then((response) => response.text()).then((text) => {
      this.documentEditor!.documentEditor.open(text);
    });
  }
}
