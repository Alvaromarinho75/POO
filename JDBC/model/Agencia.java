package model;

import java.util.ArrayList;
import java.util.List;

public class Agencia {
    private String endereco;
    private int id;
    private Banco banco;
    private List<Conta> contas;

    public Agencia(int numero, Banco banco, String endereco) {
        this.id = numero;
        this.banco = banco;
        this.endereco = endereco;
        this.contas = new ArrayList<>();
    }

    public String getEndereco() {
        return endereco;
    }

    public int getId() {
        return id;
    }

    public Banco getBanco() {
        return banco;
    }

    public List<Conta> getContas() {
        return contas;
    }

    public Conta adicionaConta(Cliente dono) {
        Conta novaConta = new Conta(this);
        novaConta.setDono(dono);
        contas.add(novaConta);
        return novaConta;
    }
}
