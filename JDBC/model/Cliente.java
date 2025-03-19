package model;

import java.util.ArrayList;
import java.util.List;

public class Cliente {
    private String nome;
    private String cpf;
    private List<Conta> contas;

    public Cliente(String nome, String cpf) {
        this.nome = nome;
        this.cpf = cpf;
        this.contas = new ArrayList<>();
    }

    public String getNome() {
        return nome;
    }

    public String getCpf() {
        return cpf;
    }

    public List<Conta> getContas() {
        return contas;
    }

    public void criaConta(Agencia agencia) {
        Conta novaConta = agencia.adicionaConta(this);
        contas.add(novaConta);
    }
}
