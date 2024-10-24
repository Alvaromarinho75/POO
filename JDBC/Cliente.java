import java.util.List;

public class Cliente {
    private String nome;
    private String cpf;
    private List<Conta> contas;

    public Cliente(String nome, String cpf, List<Conta> contas) {
        this.nome = nome;
        this.cpf = cpf;
        this.contas = contas;
    }

    public String getNome() {
        return nome;
    }

    public void setNome(String nome) {
        this.nome = nome;
    }

    public String getCpf() {
        return cpf;
    }

    public void setCpf(String cpf) {
        this.cpf = cpf;
    }

    public List<Conta> getContas() {
        return contas;
    }

    public void criaConta(Agencia agencia) {
        agencia.adicionaConta().setDono(this);
    }
}
