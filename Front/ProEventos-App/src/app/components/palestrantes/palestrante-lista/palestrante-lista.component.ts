import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { PaginatedResult, Pagination } from '@app/models/Pagination';
import { Palestrante } from '@app/models/Palestrante';
import { PalestranteService } from '@app/services/palestrante.service';
import { environment } from '@enviroments/environment';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Component({
  selector: 'app-palestrante-lista',
  templateUrl: './palestrante-lista.component.html',
  styleUrls: ['./palestrante-lista.component.scss']
})
export class PalestranteListaComponent implements OnInit {

  modalRef?: BsModalRef;

  public Palestrantes : Palestrante[] = [];
  public widthImg : number = 150;
  public marginImg : number = 2;
  public exibirImagem : boolean = true;
  public palestrantesFiltrados : Palestrante[] = [];

  public pagination = {} as Pagination;

  public palestranteId = 0;

  termoBuscaChanged: Subject<string> = new Subject<string>();

  constructor(private palestranteService: PalestranteService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
    ) { }

  public getImagem(imageName: string): string{
    if(imageName)
     return environment.apiURL + `Resources/Perfil/${imageName}`
  else
    return './assets/img/perfil.png'
  }

  public filtrarPalestrantes(evt: any): void{
    if(this.termoBuscaChanged.observers.length === 0){
      this.termoBuscaChanged.pipe(debounceTime(1000)).subscribe(
        (filtrarPor) => {
            this.spinner.show();
            this.palestranteService
            .getPalestrantes(
            this.pagination.currentPage,
            this.pagination.itemsPerPage,
            filtrarPor
            ).subscribe(
            (paginatedResult: PaginatedResult<Palestrante[]>) => {
              this.Palestrantes = paginatedResult.result;
              this.pagination = paginatedResult.pagination;
            },
            (error: any) => {
              this.spinner.hide();
              this.toastr.error('Erro ao carregar os palestrantes', 'Erro!');
            }
        ).add(() => this.spinner.hide());
        }
      )
    }
    this.termoBuscaChanged.next(evt.value);
  }

  public ngOnInit(): void{
    //Definir a paginação antes de carregar os palestrantes, a ordem importa!
    this.pagination = {currentPage: 1, itemsPerPage: 3, totalItems: 1} as Pagination;
    this.carregarpalestrantes();
  }

  public alterarImagem(): void{
    this.exibirImagem = !this.exibirImagem;
  }

  public mostraImagem(imagemURL: string): string{
    return (imagemURL !== '')
    ? `${environment.apiURL}resources/images/${imagemURL}`
    : '/assets/img/noImage.png';
  }

  public carregarpalestrantes(): void{
    /** spinner starts on init */
    this.spinner.show();
    this.palestranteService.getPalestrantes(this.pagination.currentPage, this.pagination.itemsPerPage).subscribe(
      (paginatedResult: PaginatedResult<Palestrante[]>) => {
        this.Palestrantes = paginatedResult.result;
        this.pagination = paginatedResult.pagination;
      },
      (error: any) => {
        this.spinner.hide();
        this.toastr.error('Erro ao carregar os palestrantes', 'Erro!');
      }
  ).add(() => this.spinner.hide());

  }

  public pageChanged(event): void{
    this.pagination.currentPage = event.page;
    this.carregarpalestrantes();
  }

  /*openModal(event: any, template: TemplateRef<any>, palestranteId: number) {
    this.palestranteId = palestranteId;
    event.stopPropagation();
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  confirm(): void {
    this.modalRef?.hide();
    this.spinner.show();

    this.palestranteService.deletepalestrante(this.palestranteId).subscribe(
      (result: any) => {
        console.log(result);
        if(result.message === 'Deletado'){
          this.toastr.success(`O palestrante foi deletado com sucesso!`, 'Deletado!');
          this.carregarpalestrantes();
        }
      },
      (error: any) => {
        console.error(error);
        this.toastr.error(`O palestrante de código ${this.palestranteId} não foi deletado!`, "Erro!")
      }
    ).add(() => this.spinner.hide());
  }

  decline(): void {
    this.modalRef?.hide();
  }*/

}
