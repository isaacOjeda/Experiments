import { Component, OnInit, ViewChild } from '@angular/core';
import { CustomToolbarItemModel, DocumentEditorContainerComponent, Toolbar, ToolbarItem } from '@syncfusion/ej2-angular-documenteditor';


@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {

  @ViewChild('documentEditor')
  public editorContainer?: DocumentEditorContainerComponent;

  public toolBarItems: (CustomToolbarItemModel | ToolbarItem)[] = [
    'New', 'Open',
    {
      prefixIcon: "e-de-ctnr-upload",
      tooltipText: "Guardar en Singrafos",
      text: "Guardar",
      id: "upload"
    },
    {
      prefixIcon: "e-de-ctnr-download",
      tooltipText: "Descargar en tu computadora",
      text: "Descargar",
      id: "download"
    },
    {
      prefixIcon: "e-de-ctnr-print",
      tooltipText: "Imprimir documento",
      text: "Imprimir",
      id: "print"
    },
    'Separator', 'Undo', 'Redo', 'Separator',
    'Find', 'Separator', 'Comments',
    'TrackChanges', 'Separator', 'RestrictEditing'
  ];

  public onCreate() {
    console.log('created');

  }

  public onToolbarClick(args: any): void {
    switch (args.item.id) {
      case 'upload':
        this.saveAsBlobAndUploadToServer();
        break;
      case 'download':
        this.download();
        break;
      case 'print':
        this.print();
        break;
    }
  };

  public print() {
    console.log('print');
    this.editorContainer!.documentEditor.print();


  }

  public download() {
    console.log('download');
    this.editorContainer!.documentEditor.save('Sample', 'Docx');
  }

  public saveAsBlobAndUploadToServer() {
    console.log('saveAsBlobAndUploadToServer');
    this.editorContainer!.documentEditor.saveAsBlob('Docx').then((blob) => {

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
      this.editorContainer!.documentEditor.open(text);
    });
  }
}
