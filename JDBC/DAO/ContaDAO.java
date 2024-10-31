package DAO;

import model.*;
import Conexao.*;

import java.sql.*;
import java.util.ArrayList;
import java.util.List;

public class ContaDAO {
    public void save(Conta conta) {
        String sql = "INSERT INTO contas (numero, saldo, agencia_id, cliente_id) VALUES (?, ?, ?, ?)";
        try (Connection conn = DatabaseConnection.getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql)) {
            stmt.setInt(1, conta.getNumero());
            stmt.setDouble(2, conta.getSaldo());
            stmt.setInt(3, conta.getAgencia().getId());
            stmt.setString(4, conta.getDono().getCpf());
            stmt.executeUpdate();
        } catch (SQLException e) {
            e.printStackTrace();
        }
    }

    public List<Conta> findAll() {
        List<Conta> contas = new ArrayList<>();
        String sql = "SELECT * FROM contas";
        try (Connection conn = DatabaseConnection.getConnection();
             PreparedStatement stmt = conn.prepareStatement(sql);
             ResultSet rs = stmt.executeQuery()) {
            while (rs.next()) {
                int numero = rs.getInt("numero");
                double saldo = rs.getDouble("saldo");
                int agenciaId = rs.getInt("agencia_id");
                int clienteId = rs.getInt("cliente_id");
                Agencia agencia = new AgenciaDAO().findById(agenciaId);
                Cliente cliente = new ClienteDAO().findById(clienteId);
                Conta conta = new Conta(agencia);
                conta.addSaldo(saldo);
                conta.setDono(cliente);
                contas.add(conta);
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return contas;
    }

    // Adicione métodos adicionais como findById, update, delete se necessário
}
