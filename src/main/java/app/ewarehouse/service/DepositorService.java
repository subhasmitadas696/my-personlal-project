/**
 * 
 */
package app.ewarehouse.service;

import app.ewarehouse.dto.BuyerDepositorResponse;
import app.ewarehouse.dto.DepositorResponse;
import app.ewarehouse.entity.Depositor;
import app.ewarehouse.entity.Status;
import org.springframework.data.domain.Page;
import org.springframework.data.domain.Pageable;

import java.util.Date;
import java.util.List;

/**
 * Priyanka Singh
 */
public interface DepositorService {

	public String save(String depositor);

	public Object takeAction(String buyer);

	public DepositorResponse getById(String id);

	public List<Depositor> getAll();

	public String deleteById(String id);

	public Page<BuyerDepositorResponse> getFilteredBuyers(Date fromDate, Date toDate, Status status, Pageable pageable);

	public Page<DepositorResponse> getFilteredDepositors(Date fromDate, Date toDate, Status status, Pageable pageable);

}
