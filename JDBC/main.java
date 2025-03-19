import DAO.*;
import model.*;

import java.util.List;

public class main {
    public static void main(String[] args) {
        // Criando um novo banco
        Banco banco = new Banco("Banco Exemplo");
        BancoDAO bancoDAO = new BancoDAO();
        bancoDAO.save(banco);

        // Criando uma nova agência
        Agencia agencia = new Agencia(1, banco,"Rua das Flores, 123");
        AgenciaDAO agenciaDAO = new AgenciaDAO();
        agenciaDAO.save(agencia);

        // Criando um novo cliente
        Cliente cliente = new Cliente("João Silva", "123.456.789-00");
        ClienteDAO clienteDAO = new ClienteDAO();
        clienteDAO.save(cliente);

        // Criando uma nova conta
        Conta conta = new Conta(agencia);
        conta.setDono(cliente);
        ContaDAO contaDAO = new ContaDAO();
        contaDAO.save(conta);

        // Testando a busca de todos os bancos
        System.out.println("Bancos:");
        List<Banco> bancos = bancoDAO.findAll();
        for (Banco b : bancos) {
            System.out.println("Banco: " + b.getNome());
        }

        // Testando a busca de todas as agências
        System.out.println("\nAgências:");
        List<Agencia> agencias = agenciaDAO.findAll();
        for (Agencia a : agencias) {
            System.out.println("Agência número: " + a.getId() + ", Endereço: " + a.getEndereco());
        }

        // Testando a busca de todos os clientes
        System.out.println("\nClientes:");
        List<Cliente> clientes = clienteDAO.findAll();
        for (Cliente c : clientes) {
            System.out.println("Cliente: " + c.getNome() + ", CPF: " + c.getCpf());
        }

        // Testando a busca de todas as contas
        System.out.println("\nContas:");
        List<Conta> contas = contaDAO.findAll();
        for (Conta ct : contas) {
            System.out.println("Conta número: " + ct.getNumero() + ", Saldo: " + ct.getSaldo());
        }

        // Testando a busca por ID
        System.out.println("\nBusca por ID:");
        Banco bancoBuscado = bancoDAO.findByName("Banco Exemplo");
        System.out.println("Banco buscado: " + bancoBuscado.getNome());
    }
}
