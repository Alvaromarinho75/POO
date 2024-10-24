import java.util.ArrayList;
import java.util.List;

public class Conta {
    private int numero;
    private double saldo;
    private Agencia agencia;
    private Cliente dono;

    public Conta(int numero, Agencia agencia) {
        this.numero = numero;
        this.saldo = 0;
        this.agencia = agencia;
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


}
