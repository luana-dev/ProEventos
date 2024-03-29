import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { AbstractControlOptions, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ValidatorField } from '@app/helpers/ValidatorField';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { AccountService } from '@app/services/account.service';
import { PalestranteService } from '@app/services/palestrante.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil-detalhe',
  templateUrl: './perfil-detalhe.component.html',
  styleUrls: ['./perfil-detalhe.component.scss']
})
export class PerfilDetalheComponent implements OnInit {
  @Output() ChangeFormValue = new EventEmitter();
  userUpdate = {} as UserUpdate;
  formPerf!: FormGroup;

  get f(): any{
    return this.formPerf.controls;
  }

  constructor(private fb: FormBuilder,
    public accountService: AccountService,
    public palestranteService: PalestranteService,
    private router: Router,
    private toaster: ToastrService,
    private spinner: NgxSpinnerService) { }

  ngOnInit() {
    this.validation();
    this.carregarUsuario();
    this.carregarFormulario();
  }

  private carregarFormulario(): void{
    this.formPerf.valueChanges
                 .subscribe(() => this.ChangeFormValue.emit({ ... this.formPerf.value }))
  }

  private carregarUsuario(): void {
    this.spinner.show();
    this.accountService.getUser().subscribe(
      (userRetorno: UserUpdate) => {
        console.log(userRetorno);
        this.userUpdate = userRetorno;
        this.formPerf.patchValue(this.userUpdate);
        this.toaster.success('Usuario Carregado', 'Sucesso!');
      },
      (error) => {
        console.log(error);
        this.toaster.error('Usuario invalido', 'Erro!');
        this.router.navigate(['/dashboard']);
      }
    ).add(() => this.spinner.hide());
  }

  private validation(): void{

    const formOptions: AbstractControlOptions = {
      validators: ValidatorField.MustMatch('password', 'confirmPassword')
    };

    this.formPerf = this.fb.group({
      username: [''],
      imagemURL: [''],
      titulo: ['', Validators.required],
      primeiroNome: ['', Validators.required],
      ultimoNome: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phoneNumber: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4)]],
      confirmPassword: ['', Validators.required],
      funcao: ['NaoInformado', Validators.required],
      descricao: ['']
    }, formOptions);
  }

  onSubmit(): void {
    this.atualizarUsuario();
  }

  public atualizarUsuario(){
    this.userUpdate = { ... this.formPerf.value }
    this.spinner.show();

    if(this.f.funcao.value == 'Palestrante'){
      this.palestranteService.post().subscribe(
        () => this.toaster.success('Função Palestrante Ativada!', 'Sucesso!'),
        (error) => {
          this.toaster.error('A Função Palestrante não pode ser Ativada!', 'Error');
          console.log(error);
        }
      )
    }

    this.accountService.updateUser(this.userUpdate).subscribe(
      () => this.toaster.success('Usuario atualizado', 'Sucesso!'),
      (error) => {
        this.toaster.error(error.error);
        console.log(error);
      }
    ).add(() => this.spinner.hide())
  }
}
