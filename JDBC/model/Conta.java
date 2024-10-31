package model;

public class Conta {
    private int numero;
    private double saldo;
    private Agencia agencia;
    private Cliente dono;

    public Conta(Agencia agencia) {
        this.agencia = agencia;
        this.numero = agencia.getContas().size() + 1;
        this.saldo = 0;
    }

    public int getNumero() {
        return numero;
    }

    public double getSaldo() {
        return saldo;
    }

    public void addSaldo(double valor) {
        this.saldo += valor;
    }

    public void removeSaldo(double valor) {
        this.saldo -= valor;
    }

    public void setDono(Cliente dono) {
        this.dono = dono;
    }

    public Agencia getAgencia() {
        return agencia;
    }

    public Cliente getDono() {
        return dono;
    }

}
