import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})
export class EventosComponent implements OnInit {

  public eventos : any = [];
  widthImg : number = 150;
  marginImg : number = 2;
  exibirImagem : boolean = true;
  private _filtrarLista : string = '';
  public eventosFiltrados : any = [];

  public get filtrarLista(): string{
    return this._filtrarLista;
  }

  public set filtrarLista(value: string){
    this._filtrarLista = value;
    this.eventosFiltrados = this.filtrarLista ? this.filtrarEventos(this.filtrarLista) : this.eventos;

  }

  filtrarEventos(filtrarPor: string): any{
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: any) => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1 ||
      evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    )
  }

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getEventos();
  }

  alterarImagem(){
    this.exibirImagem = !this.exibirImagem;
  }

  public getEventos(): void{
    this.http.get('https://localhost:5001/api/Eventos').subscribe(
      response => {
        this.eventos = response;
        this.eventosFiltrados = this.eventos;
      },
      error => console.log(error)
    )

  }

}
