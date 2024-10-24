import java.util.ArrayList;
import java.util.List;

public class Agencia {
    private String endereco;
    private int numero;
    private Banco banco;
    private List<Conta> contas;
    private int Ncontas = 0;

    public Agencia(int numero, Banco banco){
        this.numero = numero;
        this.banco = banco;
        this.contas = new ArrayList<>();
    }

    public String getEndereco() {
        return endereco;
    }

    public void setEndereco(String endereco) {
        this.endereco = endereco;
    }

    public int getNumero() {
        return numero;
    }

    public Banco getBanco() {
        return banco;
    }

    public List<Conta> getClientes() {
        return contas;
    }

    private int proximaConta(){
        Ncontas++;
        return Ncontas;
    }

    public Conta adicionaConta(){
        Conta novaConta = new Conta(proximaConta(), this);
        contas.add(novaConta);
        return novaConta;
    }
}
