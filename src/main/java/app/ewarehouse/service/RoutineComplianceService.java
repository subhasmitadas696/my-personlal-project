package app.ewarehouse.service;

import app.ewarehouse.dto.RoutineComplianceDTO;
import app.ewarehouse.entity.Status;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;

import java.util.Date;
import java.util.List;

public interface RoutineComplianceService {
    String save(String routineCompliance, int loggedInUserId);
    RoutineComplianceDTO getById(String Id);
    List<RoutineComplianceDTO> getAll();
    Page<RoutineComplianceDTO> getAll(Pageable pageable);
    String deleteById(String id);
    Page<RoutineComplianceDTO> findByFilters(Integer roleId, Date fromDate, Date toDate, String search, String sortColumn, String sortDirection, Integer userId, Integer action, Pageable pageable);
    public String takeAction(String data);
}
