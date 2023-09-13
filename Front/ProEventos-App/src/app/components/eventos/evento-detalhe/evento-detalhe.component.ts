import { Component, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Data, Router } from '@angular/router';
import { Evento } from '@app/models/Evento';
import { Lote } from '@app/models/Lote';
import { EventoService } from '@app/services/evento.service';
import { LoteService } from '@app/services/lote.service';
import { environment } from '@enviroments/environment';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  modalRef: BsModalRef;
  evento = {} as Evento;
  form!: FormGroup;
  estadoSalvar = 'post';
  eventoId : number;
  loteAtual = {id: 0, nome: '', indice: 0};
  imagemURL = 'assets/img/upload.png';
  file : File;

  get modoEditar(): boolean{
    return this.estadoSalvar === 'put';
  }

  get f(): any{
    return this.form.controls;
  }

  get lotes(): FormArray{
    return this.form.get('lotes') as FormArray;
  }

  get bsConfig(): any{
    return {
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY hh:mm a',
      showWeekNumbers: false,
      containerClass: 'theme-default'
    }
  }

  get bsConfigLote(): any{
    return {
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY',
      showWeekNumbers: false,
      containerClass: 'theme-default'
    }
  }

  constructor(private fb: FormBuilder,
              private localeService: BsLocaleService,
              private ActivatedRouter: ActivatedRoute,
              private eventoService: EventoService,
              private spinner: NgxSpinnerService,
              private toastr: ToastrService,
              private router: Router,
              private loteService: LoteService,
              private modalService: BsModalService)
  {
  this.localeService.use('pt-br');
  }

  public mudarValorData(value: Date, indice: number, campo: string): void{
    this.lotes.value[indice][campo] = value;
  }

  public carregarEvento(): void{
    this.eventoId = +this.ActivatedRouter.snapshot.paramMap.get('id');

    if(this.eventoId !== null && this.eventoId != 0){
      this.spinner.show();

      this.estadoSalvar = 'put';

      this.eventoService.getEventoById(this.eventoId).subscribe(
        (evento: Evento) => {
          this.evento = {... evento};
          this.form.patchValue(this.evento);
          if(this.evento.imagemURL !== ''){
            this.imagemURL = environment.apiURL + 'resources/images/' + this.evento.imagemURL;
          }
          this.carregarLotes();
        },
        (error: any) => {
          this.spinner.hide();
          this.toastr.error('Erro ao tentar carregar evento.', 'Erro!')
          console.error(error);
        },
        () => this.spinner.hide(),
      );
    }
  }

  ngOnInit(): void {
    this.carregarEvento();
    this.validation();
  }

  public validation(): void{
    this.form = this.fb.group({
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      imagemURL: [''],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      lotes : this.fb.array([])
    });
  }
  resetForm(): void{
    this.form.reset();
  }

  public cssValidator(campoForm: FormControl | AbstractControl) : any {
    return {'is-invalid': campoForm.errors && campoForm.touched }
  }

  public salvarEventos(): void{
    if(this.form.controls.lotes.valid){
    this.spinner.show();
  }

    if(this.form.valid){
      this.evento = (this.estadoSalvar === 'post')
                    ? {... this.form.value}
                    : {id: this.evento.id, ... this.form.value};

      this.eventoService[this.estadoSalvar](this.evento).subscribe(
        (eventoRetorno: Evento) => {
          this.router.navigate([`eventos/detalhe/${eventoRetorno.id}`])
          this.toastr.success('O evento foi salvo com sucesso!', 'Sucesso')
        },
        (error: any) => {
          console.error(error);
          this.toastr.error('O evento não foi salvo!', 'Erro!');
        }
      ).add(() => this.spinner.hide());
    }
  }

  adicionarLotes(): void{
    this.lotes.push(this.criarLote({id: 0} as Lote));
  }

  criarLote(lote: Lote): FormGroup{
    return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      preco: [lote.preco, Validators.required],
      dataini: [lote.dataInicio],
      datafim: [lote.dataFim],
      qtd: [lote.quantidade, Validators.required]
    });
  }

  public salvarLotes(): void{
    this.spinner.show();
    if(this.form.controls.lotes.valid){
      console.log(this.form.value.lotes)
      this.loteService.saveLote(this.eventoId, this.form.value.lotes)
                      .subscribe(
                        () => {
                          this.toastr.success('Lote adicionado com sucesso!', 'Sucesso!');
                        },
                        (error: any) => {
                          this.toastr.error('Erro ao adicionar o Lote!', 'Erro!');
                          console.log(error);
                        }
                      ).add(() => this.spinner.hide());
    }
  }

  public carregarLotes(): void{
    this.loteService.getLotesByEventoId(this.eventoId).subscribe(
      (lotesRetorno: Lote[]) => {
        lotesRetorno.forEach(lote => {
          this.lotes.push(this.criarLote(lote));
        });
      },
      (error: any) => {
        this.toastr.error('Erro ao tentar carregar lote.', 'Erro!');
        console.log(error);
      }
    ).add(() => this.spinner.hide())
  }

  public removerLote(template: TemplateRef<any>, indice: number): void{
    this.loteAtual.id = this.lotes.get(indice + '.id').value;
    this.loteAtual.nome = this.lotes.get(indice + '.nome').value;
    this.loteAtual.indice = indice;

    this.modalRef = this.modalService.show(template, {class:'modal-sm'})
  }

  public retornaTituloLote(tituloLote: string): string{
    return  tituloLote === null || tituloLote === '' ? 'Nome do Lote' : tituloLote;
  }

  public confirmDelete(): void{
    this.modalRef.hide();
    this.spinner.show();

    this.loteService.deleteLote(this.eventoId, this.loteAtual.id).subscribe(
      () => {
        this.spinner.hide();
        this.lotes.removeAt(this.loteAtual.indice);
        this.toastr.success('Sucesso ao deletar o lote.', 'Sucesso!');
      },
      (error: any) => {
        this.toastr.error('Erro ao tentar deletar o lote', 'Erro!');
        console.log(error);
      }
    ).add(() => this.spinner.hide())
  }

  public declineDelete(): void{
    this.modalRef.hide();
  }

  onFileChange(ev: any): void{
    const reader = new FileReader();

    reader.onload = (event: any) => this.imagemURL = event.target.result;

    this.file = ev.target.files;
    reader.readAsDataURL(this.file[0]);

    this.uploadImage();
  }

  uploadImage(): void{
    this.spinner.show();
    this.eventoService.postUpload(this.eventoId, this.file).subscribe(
      () => {
        this.carregarEvento();
        this.toastr.success('Sucesso ao carregar a imagem!', 'Sucesso!');
      },
      (error: any) => {
        this.toastr.error('Erro ao carregar imagem', 'Error!');
        console.log(error)
      }
    ).add(() => this.spinner.hide)

  }
}
