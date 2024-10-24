import java.util.ArrayList;
import java.util.List;

public class Banco {
    private String nome;
    private List<Agencia> agencias;
    private int Nagencias = 0;

    public Banco(String nome){
        this.nome = nome;
        this.agencias = new ArrayList<>();
    }

    public String getNome() {
        return nome;
    }

    public void setNome(String nome) {
        this.nome = nome;
    }

    public List<Agencia> getAgencias() {
        return agencias;
    }

    private int proximaAgencia(){
        Nagencias++;
        return Nagencias;
    }

    public void adicionaAgencia(){
        Agencia novaAgencia = new Agencia(proximaAgencia(), this);
        agencias.add(novaAgencia);
    }
}
