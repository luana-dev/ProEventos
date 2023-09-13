import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { environment } from '@enviroments/environment';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {

  modalRef?: BsModalRef;

  public eventos : Evento[] = [];
  public widthImg : number = 150;
  public marginImg : number = 2;
  public exibirImagem : boolean = true;
  private _filtrarLista : string = '';
  public eventosFiltrados : Evento[] = [];

  public eventoId = 0;

  public get filtrarLista(): string{
    return this._filtrarLista;
  }

  public set filtrarLista(value: string){
    this._filtrarLista = value;
    this.eventosFiltrados = this.filtrarLista ? this.filtrarEventos(this.filtrarLista) : this.eventos;

  }

  public filtrarEventos(filtrarPor: string): Evento[]{
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: any) => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1 ||
      evento.local.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    )
  }

  constructor(private eventoService: EventoService,
              private modalService: BsModalService,
              private toastr: ToastrService,
              private spinner: NgxSpinnerService,
              private router: Router
              ) { }

  public ngOnInit(): void{
    this.carregarEventos();
/** spinner starts on init */
    this.spinner.show();
  }

  public alterarImagem(): void{
    this.exibirImagem = !this.exibirImagem;
  }

  public mostraImagem(imagemURL: string): string{
    return (imagemURL !== '')
    ? `${environment.apiURL}resources/images/${imagemURL}`
    : '/assets/img/noImage.png';
  }

  public carregarEventos(): void{
    this.eventoService.getEvento().subscribe({
      next: (eventos: Evento[]) => {
        this.eventos = eventos;
        this.eventosFiltrados = this.eventos;
      },
      error: (error: any) =>{
        this.spinner.hide();
        this.toastr.error('Erro ao carregar os eventos', 'Erro!');
      },
      complete: () => this.spinner.hide()
  })

  }

  openModal(event: any, template: TemplateRef<any>, eventoId: number) {
    this.eventoId = eventoId;
    event.stopPropagation();
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  confirm(): void {
    this.modalRef?.hide();
    this.spinner.show();

    this.eventoService.deleteEvento(this.eventoId).subscribe(
      (result: any) => {
        console.log(result);
        if(result.message === 'Deletado'){
          this.toastr.success(`O evento foi deletado com sucesso!`, 'Deletado!');
          this.carregarEventos();
        }
      },
      (error: any) => {
        console.error(error);
        this.toastr.error(`O evento de código ${this.eventoId} não foi deletado!`, "Erro!")
      }
    ).add(() => this.spinner.hide());
  }

  decline(): void {
    this.modalRef?.hide();
  }

  detalheEvento(id: number): void{
    this.router.navigate([`eventos/detalhe/${id}`]);
  }

}
